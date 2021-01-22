# Big numbers
Big numbers in Sigiri have unlimited precision. Which means you can use any amount of digits in you integer number. In other words, you can use any large positive or negative numbers in Sigiri.

### How to define a big number?
There are two main ways of doing that. When you insert a large number literal in your code, it will automatically converted into a big number. Or you can use `big` keyword to define a big number.
```sh
x = 123456789123456789123456789123456789123456789			    
x:big = 10
```

### Attributes of a complex object
- None for now

### Associated methods
Associated methods with big numbers are provided by `system.math` library. Therfore, you have to load it first to use below methods. Example,
```sh
load system.math

x:big = -15
math.bpow(x, 2)
math.blog(x)
```
- `bpow(value, power)` : The value is raised to the value of power.
- `blog(value)` : The base `e` logarithm of value.
- `blog(value, baseValue)` : The logarithm of value in baseValue.
- `blog10(value)` : The base-10 logarithm of value.

### Notes
- The built in method `abs(value)` can be used to get the absolute value of a big number.