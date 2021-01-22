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
Every complex number objects will have following read-only attributes. you can access them as normal attribute. For example,
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