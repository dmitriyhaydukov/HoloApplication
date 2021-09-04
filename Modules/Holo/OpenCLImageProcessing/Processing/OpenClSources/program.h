#define PI 3.14159265358979323846	
#define PI_2 1.57079632679489661923	

//Вычисление поворачивающего модуля e^(-i*2*PI*k/N), W
__kernel void spinFact(__global float2* w, int n)
{
	unsigned int i = get_global_id(0);

	float2 angle = (float2)(2 * i*PI / (float)n, (2 * i*PI / (float)n) + PI_2);
	w[i] = cos(angle);
}

__kernel void bitReverseKernel(__global float2 *dst, __global float2 *src, int m, int n)
{
	unsigned int gid = get_global_id(0);
	unsigned int nid = get_global_id(1);

	unsigned int j = gid;
	j = (j & 0x55555555) << 1 | (j & 0xAAAAAAAA) >> 1;
	j = (j & 0x33333333) << 2 | (j & 0xCCCCCCCC) >> 2;
	j = (j & 0x0F0F0F0F) << 4 | (j & 0xF0F0F0F0) >> 4;
	j = (j & 0x00FF00FF) << 8 | (j & 0xFF00FF00) >> 8;
	j = (j & 0x0000FFFF) << 16 | (j & 0xFFFF0000) >> 16;

	j >>= (32 - m);

	dst[nid*n + j] = src[nid*n + gid];
}

__kernel void norm(__global float2 *x, int n)
{
	unsigned int gid = get_global_id(0);
	unsigned int nid = get_global_id(1);

	x[nid*n + gid] = x[nid*n + gid] / (float2)((float)n, (float)n);
}

// m <=> 2^m = n
__kernel void butterfly(__global float2 *x, __global float2* w, int m, int n, int iter, uint flag)
{
	unsigned int gid = get_global_id(0);
	unsigned int nid = get_global_id(1);

	int butterflySize = 1 << (iter - 1);
	int butterflyGrpDist = 1 << iter;
	int butterflyGrpNum = n >> iter;
	int butterflyGrpBase = (gid >> (iter - 1))*(butterflyGrpDist);
	int butterflyGrpOffset = gid & (butterflySize - 1);

	int a = nid * n + butterflyGrpBase + butterflyGrpOffset;
	int b = a + butterflySize;

	int l = butterflyGrpNum * butterflyGrpOffset;

	float2 xa, xb, xbxx, xbyy, wab, wayx, wbyx, resa, resb;

	xa = x[a];
	xb = x[b];
	xbxx = xb.xx;
	xbyy = xb.yy;

	wab = as_float2(as_uint2(w[l]) ^ (uint2)(0x0, flag));
	wayx = as_float2(as_uint2(wab.yx) ^ (uint2)(0x80000000, 0x0));
	wbyx = as_float2(as_uint2(wab.yx) ^ (uint2)(0x0, 0x80000000));

	resa = xa + xbxx*wab + xbyy*wayx;
	resb = xa - xbxx*wab + xbyy*wbyx;

	x[a] = resa;
	x[b] = resb;
}

__kernel void transpose(__global float2 *dst, __global float2* src, int n)
{
	unsigned int xgid = get_global_id(0);
	unsigned int ygid = get_global_id(1);

	unsigned int iid = ygid * n + xgid;
	unsigned int oid = xgid * n + ygid;

	dst[oid] = src[iid];
}

__kernel void highPassFilter(__global float2* image, int n, int radius)
{
	unsigned int xgid = get_global_id(0);
	unsigned int ygid = get_global_id(1);

	int2 n_2 = (int2)(n >> 1, n >> 1);
	int2 mask = (int2)(n - 1, n - 1);

	int2 gid = ((int2)(xgid, ygid) + n_2) & mask;

	int2 diff = n_2 - gid;
	int2 diff2 = diff * diff;
	int dist2 = diff2.x + diff2.y;

	int2 window;

	if (dist2 < radius*radius) {
		window = (int2)(0L, 0L);
	}
	else {
		window = (int2)(-1L, -1L);
	}
	image[ygid*n + xgid] = as_float2(as_int2(image[ygid*n + xgid]) & window);
}

// скалярное произведение векторов
float vectorMul(float4 v1, float4 v2)
{
   return v1.x*v2.x + v1.y*v2.y + v1.z*v2.z + v1.w * v2.w;
}

float amplitude(float2 v)
{
	return sqrt(v.x*v.x + v.y*v.y);
}

float phase(float2 v)
{
	return atan2(v.y, v.x);
}

// умножение двух комплексных чисел
float2 mul(float2 a,float2 b)
{
#if USE_MAD
  return (float2)(mad(a.x,b.x,-a.y*b.y),mad(a.x,b.y,a.y*b.x)); // mad
#else
  return (float2)(a.x*b.x-a.y*b.y,a.x*b.y+a.y*b.x); // no mad
#endif
}


#define MUL(v1,v2) (v1.x*v2.x + v1.y*v2.y + v1.z*v2.z + v1.w * v2.w)

float2 complexFromPolarCoordinates(float magnitude, float phase)
{
    return (float2)(magnitude * cos(phase), magnitude * sin(phase));
}

__kernel void psi4Kernel(__read_only image2d_t img0, __read_only image2d_t img1, __read_only image2d_t img2, __read_only image2d_t img3, float4 k_sin, float4 k_cos, float znmt_abs, float amplitude, __write_only image2d_t output)
{
    const sampler_t smp = CLK_NORMALIZED_COORDS_FALSE | //Natural coordinates
         CLK_ADDRESS_CLAMP | //Clamp to zeros
         CLK_FILTER_NEAREST; //Don't interpolate
    int2 coord = (int2)(get_global_id(0), get_global_id(1));
    float4 val0 = read_imagef(img0, smp, coord);
    float4 val1 = read_imagef(img1, smp, coord);
    float4 val2 = read_imagef(img2, smp, coord);
    float4 val3 = read_imagef(img3, smp, coord);
    float4 i_sdv = (float4)(val0.x, val1.x, val2.x, val3.x);

    float fz1 = vectorMul(i_sdv, k_sin);
    float fz2 = vectorMul(i_sdv, k_cos);
    float a = atan2(fz2, fz1);

    float am = sqrt(fz1*fz1 + fz2*fz2) / znmt_abs;
    //am = am / (2 * amplitude);

    float2 result = complexFromPolarCoordinates(am, a);
    write_imagef(output, coord, (float4)(result.x, result.y, 0, 0));
}

// minmax for 2 channels
__kernel void minMax2(__read_only image2d_t img, __global float2* minBuffer, __global float2* maxBuffer, int workSize, int width, int height, int bufferWidth)
{
    const sampler_t smp = CLK_NORMALIZED_COORDS_FALSE | //Natural coordinates
         CLK_ADDRESS_CLAMP | //Clamp to zeros
         CLK_FILTER_NEAREST; //Don't interpolate
    
	int2 coord = (int2)(get_global_id(0), get_global_id(1));
    
    int2 iCoord = coord * workSize;
    int xSize = workSize;
    if (iCoord.x + workSize > width)
        xSize = width - iCoord.x;

    int ySize = workSize;
    if (iCoord.x + workSize > width)
        ySize = height - iCoord.x;

    float2 minV = read_imagef(img, smp, iCoord).xy;
    float2 maxV = minV;  
    
    for (int dx = 1; dx < xSize; dx++)
    {
        for (int dy = 1; dy < ySize; dy++)
        {
            float2 v = read_imagef(img, smp, (int2)(iCoord.x + dx, iCoord.y + dy)).xy;
            if (minV.x > v.x)
                minV.x = v.x;
            if (minV.y > v.y)
                minV.y = v.y;

            if (maxV.x < v.x)
                maxV.x = v.x;
            if (maxV.y < v.y)
                maxV.y = v.y;
        }
    }

    minBuffer[coord.x + coord.y * bufferWidth] = minV;
    maxBuffer[coord.x + coord.y * bufferWidth] = maxV;
}

__kernel void reduce(__read_only image2d_t img, __global float* buffer, int workSize, int width, int height, int bufferWidth)
{
    const sampler_t smp = CLK_NORMALIZED_COORDS_FALSE | //Natural coordinates
         CLK_ADDRESS_CLAMP | //Clamp to zeros
         CLK_FILTER_NEAREST; //Don't interpolate
    
	int2 coord = (int2)(get_global_id(0), get_global_id(1));
    
    int2 iCoord = coord * workSize;
    int xSize = workSize;
    if (iCoord.x + workSize > width)
        xSize = width - iCoord.x;

    int ySize = workSize;
    if (iCoord.x + workSize > width)
        ySize = height - iCoord.x;

    float av = 0;  
	int cnt = 0;
    
    for (int dx = 1; dx < xSize; dx++)
    {
        for (int dy = 1; dy < ySize; dy++)
        {
            float v = read_imagef(img, smp, (int2)(iCoord.x + dx, iCoord.y + dy)).x;
			
			av += v;
			cnt++;
        }
    }

    buffer[coord.x + coord.y * bufferWidth] = av / cnt;
}

__kernel void copyImageToBuffer(__read_only image2d_t img, __global float* buffer)
{
	const sampler_t smp = CLK_NORMALIZED_COORDS_FALSE | //Natural coordinates
         CLK_ADDRESS_CLAMP | //Clamp to zeros
         CLK_FILTER_NEAREST; //Don't interpolate
    
	int2 coord = (int2)(get_global_id(0), get_global_id(1));
	int w = get_global_size(0);

	buffer[coord.x + coord.y * w] = read_imagef(img, smp, coord).x;
}

__kernel void copyImageToBuffer2(__read_only image2d_t img, __global float2* buffer)
{
	const sampler_t smp = CLK_NORMALIZED_COORDS_FALSE | //Natural coordinates
         CLK_ADDRESS_CLAMP | //Clamp to zeros
         CLK_FILTER_NEAREST; //Don't interpolate
    
	int2 coord = (int2)(get_global_id(0), get_global_id(1));
	int w = get_global_size(0);

	buffer[coord.x + coord.y * w] = read_imagef(img, smp, coord).xy;
}

__kernel void transpose_img(__read_only image2d_t input, __write_only image2d_t output)
{
	const sampler_t smp = CLK_NORMALIZED_COORDS_FALSE | //Natural coordinates
         CLK_ADDRESS_CLAMP | //Clamp to zeros
         CLK_FILTER_NEAREST; //Don't interpolate
    
	int2 coord = (int2)(get_global_id(0), get_global_id(1));
	float4 val = read_imagef(input, smp, coord);
	write_imagef(output, (int2)(coord.y, coord.x), val);
}

// w is half of width
// h is half of height
__kernel void cyclicShift_img(__read_only image2d_t input, __write_only image2d_t output)
{
	int x = get_global_id(0);
	int y = get_global_id(1);
	int w = get_global_size(0);
	int h = get_global_size(1);

	int2 ctl = (int2)(x, y);
	int2 ctr = (int2)(x + w, y);
	int2 cbl = (int2)(x, y + h);
	int2 cbr = (int2)(x + w, y + h);

	const sampler_t smp = CLK_NORMALIZED_COORDS_FALSE | //Natural coordinates
         CLK_ADDRESS_CLAMP | //Clamp to zeros
         CLK_FILTER_NEAREST; //Don't interpolate

	float4 vtl = read_imagef(input, smp, ctl);
	float4 vtr = read_imagef(input, smp, ctr);
	float4 vbl = read_imagef(input, smp, cbl);
	float4 vbr = read_imagef(input, smp, cbr);

	write_imagef(output, ctl, vbr);
	write_imagef(output, ctr, vbl);
	write_imagef(output, cbl, vtr);
	write_imagef(output, cbr, vtl);
}

// lambda - длина волны, в мкм
// d - расстояние до объекта, в мм
// nx - размер изображения в пикселях
// dx - размер изображения в мм
__kernel void freshnelGenerateInnerMultipliers(__global float2 *dst, float lambda, float d, float nx, float dx)
{
	int x = get_global_id(0);

	float deltax = dx / nx;
	float k = deltax * (nx / 2 - (float)x);
	k = PI / (lambda * d) * k * k;
	dst[x] = (float2)(cos(k), sin(k));
}

//__kernel void flatWavefront(float alpha, float amplitude, __read_only image2d_t output)
//{
//    int2 coord = (int2)(get_global_id(0), get_global_id(1));
//    float phase = coord.x * sin(alpha);
//    write_imagef(output, coord, (float4)(amplitude * cos(phase), amplitude * sin(phase), 0, 0));
//}

__kernel void sphericWavefront(__write_only image2d_t output, float lambda, float d, float dx, float dy, float alphaX, float alphaY, float amplitude)
{
	int2 coord = (int2)(get_global_id(0), get_global_id(1));
	int2 size = (int2)(get_global_size(0), get_global_size(1));
	float deltax = dx / size.x;
	float deltay = dy / size.y;
	//float k = PI / lambda;
	float k = PI / (lambda * d);
	float adx = tan(alphaX)*d;
	float ady = tan(alphaY)*d;
	float kx = deltax * (size.x / 2 + adx - (float)coord.x);
	float ky = deltay * (size.y / 2 + ady - (float)coord.y);
	float dxy = kx*kx+ky*ky;	

	k = k*dxy;
	write_imagef(output, coord, (float4)(cos(k), sin(k), 0, 0));
}

// умножение изображения на внутренний множитель (внутри преобразования Фурье) - см. Глава 4
// fx - массив частей коэффициента (экспоненты) меняющихся по оси х (freshnelGenerateInnerMultipliers)
// fy - массив частей коэффициента меняющихся по оси y
__kernel void freshnelMultiplyInner(__read_only image2d_t input, __write_only image2d_t output, __global const float2 * fx, __global const float2 * fy)
{
	int x = get_global_id(0);
	int y = get_global_id(1);

	// TODO: убрать в define
	const sampler_t smp = CLK_NORMALIZED_COORDS_FALSE | //Natural coordinates
         CLK_ADDRESS_CLAMP | //Clamp to zeros
         CLK_FILTER_NEAREST; //Don't interpolate
	
	int2 coord = (int2)(x, y);
	float2 val = read_imagef(input, smp, coord).xy;
	float2 fxv = fx[x];
	float2 fyv = fy[y];
	float2 fxyv = mul(fxv, fyv);
	//val = val * fx[x] * fy[y];
	val = mul(val, fxyv);
	write_imagef(output, coord, (float4)(val.x, val.y, 0, 0));
}

__kernel void shift(__read_only image2d_t input, __write_only image2d_t output, int dx, int dy, int cyclic)
{
	int x = get_global_id(0);
	int y = get_global_id(1);
	int w = get_global_size(0);
	int h = get_global_size(1);

	int x0 = x - dx;
	int y0 = y - dy;

	if (x0 < 0 && cyclic)
	{
		x0 = w + x0;
	}

	if (y0 < 0 && cyclic)
	{
		y0 = h + y0;
	}

	float2 val;

	if (y0 >= 0 && x0 >= 0)
	{
		const sampler_t smp = CLK_NORMALIZED_COORDS_FALSE | //Natural coordinates
			 CLK_ADDRESS_CLAMP | //Clamp to zeros
			 CLK_FILTER_NEAREST; //Don't interpolate

		val = read_imagef(input, smp, (int2)(x0, y0)).xy;
	}
	else
	{
		val = (float2)(0,0);
	}

	write_imagef(output, (int2)(x, y), (float4)(val.x, val.y, 0, 0));
}

// https://www.fxyz.ru/%D1%84%D0%BE%D1%80%D0%BC%D1%83%D0%BB%D1%8B_%D0%BF%D0%BE_%D0%BC%D0%B0%D1%82%D0%B5%D0%BC%D0%B0%D1%82%D0%B8%D0%BA%D0%B5/%D0%BA%D0%BE%D0%BC%D0%BF%D0%BB%D0%B5%D0%BA%D1%81%D0%BD%D1%8B%D0%B5_%D1%87%D0%B8%D1%81%D0%BB%D0%B0/%D0%B4%D0%B5%D0%BB%D0%B5%D0%BD%D0%B8%D0%B5_%D0%BA%D0%BE%D0%BC%D0%BF%D0%BB%D0%B5%D0%BA%D1%81%D0%BD%D1%8B%D1%85_%D1%87%D0%B8%D1%81%D0%B5%D0%BB/
__kernel void divide(__read_only image2d_t num, __read_only image2d_t den, __write_only image2d_t output)
{
	int x = get_global_id(0);
	int y = get_global_id(1);
	int2 coord = (int2)(x, y);

	const sampler_t smp = CLK_NORMALIZED_COORDS_FALSE | //Natural coordinates
         CLK_ADDRESS_CLAMP | //Clamp to zeros
         CLK_FILTER_NEAREST; //Don't interpolate

	float2 val1 = read_imagef(num, smp, coord).xy;
	float2 val2 = read_imagef(den, smp, coord).xy;

	float a = val1.x;
	float b = val1.y;
	float ah = val2.x;
	float bh = val2.y;
	float d = ah * ah + bh * bh;

	float2 result = (float2)((a*ah+b*bh)/d,(ah*b-bh*a)/d);

	write_imagef(output, coord, (float4)(result.x, result.y, 0, 0));
}

__kernel void divideByNum(__read_only image2d_t input, __write_only image2d_t output, float num)
{
	int x = get_global_id(0);
	int y = get_global_id(1);
	int2 coord = (int2)(x, y);

	const sampler_t smp = CLK_NORMALIZED_COORDS_FALSE | //Natural coordinates
         CLK_ADDRESS_CLAMP | //Clamp to zeros
         CLK_FILTER_NEAREST; //Don't interpolate

	float2 val = read_imagef(input, smp, coord).xy;

	write_imagef(output, coord, (float4)(val.x / num, val.y / num, 0, 0));
}

__kernel void sum(__read_only image2d_t input1, __read_only image2d_t input2, __write_only image2d_t output)
{
	int x = get_global_id(0);
	int y = get_global_id(1);
	int2 coord = (int2)(x, y);

	const sampler_t smp = CLK_NORMALIZED_COORDS_FALSE | //Natural coordinates
         CLK_ADDRESS_CLAMP | //Clamp to zeros
         CLK_FILTER_NEAREST; //Don't interpolate

	float2 val1 = read_imagef(input1, smp, coord).xy;
	float2 val2 = read_imagef(input2, smp, coord).xy;	

	float2 result = val1 + val2;

	write_imagef(output, coord, (float4)(result.x, result.y, 0, 0));
}

__kernel void combineAmplitudeAndPhase(__read_only image2d_t ampImg, __read_only image2d_t phImg, __write_only image2d_t output)
{
	int x = get_global_id(0);
	int y = get_global_id(1);
	int2 coord = (int2)(x, y);

	const sampler_t smp = CLK_NORMALIZED_COORDS_FALSE | //Natural coordinates
         CLK_ADDRESS_CLAMP | //Clamp to zeros
         CLK_FILTER_NEAREST; //Don't interpolate

	float2 ampCpx = read_imagef(ampImg, smp, coord).xy;
	float2 phCpx = read_imagef(phImg, smp, coord).xy;	

	float amp = sqrt(ampCpx.x*ampCpx.x + ampCpx.y*ampCpx.y);
	float ph = atan2(phCpx.y, phCpx.x);

	float re = amp * cos(ph);
	float im = amp * sin(ph);

	write_imagef(output, coord, (float4)(re, im, 0, 0));
}

__kernel void interference(__read_only image2d_t img1, __read_only image2d_t img2, __write_only image2d_t output)
{
	int x = get_global_id(0);
	int y = get_global_id(1);
	int2 coord = (int2)(x, y);

	const sampler_t smp = CLK_NORMALIZED_COORDS_FALSE | //Natural coordinates
         CLK_ADDRESS_CLAMP | //Clamp to zeros
         CLK_FILTER_NEAREST; //Don't interpolate

	float2 val1 = read_imagef(img1, smp, coord).xy;
	float2 val2 = read_imagef(img2, smp, coord).xy;
	
	float a1 = amplitude(val1);
	float a2 = amplitude(val2);
	float p1 = phase(val1);
	float p2 = phase(val2);

	float i = a1*a1+a2*a2+2*a1*a2*cos(p1-p2);

	write_imagef(output, coord, (float4)(i, 0, 0, 0));
}