# Complex numbers
As well as the real numbers, Sigiri can handle complex numbers. You can manipulate complex numbers using methods provided by `system.math` library.

### How to define a complex number?
There are two main ways of doing that. You can define a complex number using inline literals or using built-in `complex(x, y)` method. 
Complex numbers are denoted by x+yi where x is the real part and y is the imaginary part, i is just the symbol showing that, this is a complex number.
```sh
c = 1+2i			    
c = complex(1, 2)
```

### Attributes of a complex object
Every complex number object will have following read-only attributes. you can access them as normal attribute. For example,
```sh
c = 2+0i
c.real // getting real part (here its 2)
c.imag // getting imaginary part (here its 0)
c.phase // getting the phase
```
- `real` : gives the real part of the complex number.
- `imag` : gives the imaginary part of the complex number.
- `phase` : gives the phase of the complex number.
- `mag` : gives the magnitude of the complex number.
- `conj` : gives the conjugate of the complex number.
- `isFinite` : true if both real and imaginary parts are finite.
- `isInf` : true if either, the real or imaginary part is infinite.
- `isNan` : true if the value is not finite nor infinite.

### Associated methods
Associated methods with complex numbers are provided by `system.math` library. Therfore, you have to load it first to use below methods. Example,
```sh
load system.math

c = 1+1i
math.cacos(c)
math.ccos(c)
```
- `cacos(value)` : The arc cosine of value.
- `casin(value)` : The arc sine of value.
- `catan(value)` : The arc tangent of value.
- `cconj(value)` : Conjugate of value.
- `ccos(value)` : The cosine of value.
- `ccosh(value)` : The hyperbolic cosine of value.
- `cexp(value)` : The constant `e` raised to the power of value.
- `cFromPolar(mag, phase)` : Creates a complex number from polar coordinates.
- `cIsFinite(value)` : Returns true if both real and imaginary parts are finite of value.
- `cIsInf(value)` : Returns true if either, the real or imaginary part is infinite of value.
- `cIsNan(value)` : Returns true if value is not finite nor infinite.
- `clog(value)` : The base `e` logarithm of value.
- `clog(value, baseValue)` : The logarithm of value in baseValue.
- `log10(value)` : The base-10 logarithm of value.
- `cnegate(value)` : Both real and imaginary parts of value are multiplied by -1.
- `cpower(value, power)` : The value is raised to the value of power.
- `creci(value)` : Reciprocal of value.
- `csin(value)` : The sine of value.
- `csinh(value)` : The hyperbolic sine of value.
- `csqrt(value)` : The square root of value.
- `ctan(value)` : The tangent of value.
- `ctanh(value)` : The hyperbolic tangent of value.

### Notes
- The built in method `abs(value)` can used to get the absolute value of a complex number.
- Fractional exponent of negative numbers, will also return a complex number.
```sh
c = (-10)**0.5
```
After executing above code, the variable c will get the result of expression `(-10)**0.5` which is a complex number.
