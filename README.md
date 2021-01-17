# Sigiri
Sigiri is a simple, object-oriented, interpreted programming laguage. Designed for fun. Due to performance issues and platform compatibility issues, this project will also get developed using C++ [here.](https://github.com/YohanSandun/CSigiri) Both repositories ([Sigiri](https://github.com/YohanSandun/Sigiri) and [CSigiri](https://github.com/YohanSandun/CSigiri)) will continue to develop simultaneously. Those who are not familiar with C++ can follow on this repository. 

**Note:** Following documentation is specialized for this repository.

#### Highlights
- Object-Oriented programming
- Dynamically typed
- Can interface managed assemblies (.dll/.so)
- Cross-Platform (Source can be compiled on Windows,Linux, and OSX using .NET Core)

#### Hello world
Hello world code is identical to Python :D
```sh
print("Hello world!")
```
#### Method example
```sh
method factorial(n) {
    if n == 0 {
        return 1
    }
    return n * factorial(n - 1)
}
print(factorial(5))
```
Above code can be written as below. Both codes give same output.
```sh
method factorial(n): if n == 0: return 1 else: return n * factorial(n - 1)
print(factorial(5))
```
#### Class example
```sh
class Point {
    method init(x, y) {
        this.x = x
        this.y = y
    }
}
pointA = Point(10, 20)
print("X position: " + pointA.x);
```
In above code `init` method is the constructor of the `Point` class. Constructor method must have `init` as the identifier. But constructor is not mandatory for classes. There can be classes without a constructor.
`this` keyword is used to access the current instance of the class. 
Methods and attributes of a class can be accessd or modified using `.` operator.

#### Inherit example
```sh
calss Animal {
    method init(kind) {
        this.kind = kind
    }
    method makeSound(): print("Grrrrrrr")
}
class Cat : Animal {
    method init (name) {
        base.init("Cat")
        this.name = name
    }
    method makeSound() : print("Meow Meow")
}
class Dog : Animal {
    method init (name) {
        base.init("Dog")
        this.name = name
    }
}
myCat = Cat("Kitty")
myCat.makeSound()
myDog = Dog("Browny")
myDog.makeSound()
```
In above code `Animal` class is the super/parent class. Other two classes are inherited from that super class. `base` keyword is used to access the super class instance.
`makeSound()` method calling on `myCat` object will invoke the **overrided** `makeSound()` method defined in the `Cat` class and it will output "Meow Meow". `makeSound()` method calling on `myDog` object will invoke the `makeSound()` method defined in the parent `Animal` class and it will output "Grrrrrrr".

#### Default values for arguments
```sh
method greet(name="NoOne", msg="Hello, ") {
    print(msg + name)
}
greet("John", "Hi, ")
greet("John")
greet()
greet(msg:"Welcome, ")
greet(name:"John")
```
In above code all ways of calling `greet` method, are possible.

#### Loading assemblies of library
```sh
load system.math
print(math.PI)
print(math.acos(0))
```
`load` keyword can be used to import another Sigiri source or compiled managed library (.dll or .so) in to our program. In above example we are using `math` class from compiled .NET assembly called `system`. (To use a certain library, that library should exists in program location. In above example I have system.dll file and source code in the same directory.)
