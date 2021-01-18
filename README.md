# Sigiri
Sigiri is a simple, object-oriented, interpreted programming laguage. Designed for fun. Due to performance issues and platform compatibility issues, this project will also get developed using C++ [here.](https://github.com/YohanSandun/CSigiri) Both repositories ([Sigiri](https://github.com/YohanSandun/Sigiri) and [CSigiri](https://github.com/YohanSandun/CSigiri)) will continue to develop simultaneously. Those who are not familiar with C++ can follow on this repository. 

**Note:** Following documentation is specialized for this repository.

### Requirements
- .NET Core 3.1 or higher

### Compiling from source
- Microsoft Windows
<br>Open Visual Studio 2019 and choose clone git repository. Then copy and paste this link:https://github.com/YohanSandun/Sigiri and click on clone button. It should clone the repository. Then simply right click on the solution and choose Rebuild. It will rebuild the project and generate necessary binaries for you.<br><br>If youre plan to use libraries: Makesure to copy all the files from libaries/ directory to your bin\Debug\netcoreapp3.1\ directory where Sigiti.exe located. And check whether that `system.dll` file exists on same bin/ directory. It should be there if build process was success. If it was not there, then check Sigiri\system\bin\Debug\netcoreapp3.1\ folder and copy system.dll into the same location you copied files from libraris/ directory.

- Linux Based and OSX
<br>Use following command to clone the repository.
```sh
git clone https://github.com/YohanSandun/Sigiri
```
Other instructions comming soon...

### Highlights
- Object-Oriented programming
- Dynamically typed
- Can interface managed assemblies (.dll/.so)
- Cross-Platform (Source can be compiled on Windows,Linux, and OSX using .NET Core)

### Built-in data types
- `integer`: can hold a positive or negative integer number (default size is 4 bytes)
- `boolean`: subtype of integer. 1 and 0 represented as `true` and `false`
- `float`: can hold a floating point number (default size is 8 bytes)
- `string`: can hold character sequence. denoted by double quotes ("Hello") or single quotes ('Hello')
- `list`: can hold sequence of objects of any type. denoted by square brackets ([1, 2, 3])
- `dictionary`: can hold collection of key:value pairs. denoted by curly braces ({'a':10, 'b':20})
---------------------------------

### Arithmetic operators
Below are the Arithmetic operators:
| Operator |                               Description                              | Syntax |
|:--------:|:----------------------------------------------------------------------:|:------:|
|     +    | Addition: Adds two operands                                            |   a+b  |
|     -    | Subtraction: Subtract second operand from first one                    |   a-b  |
|     *    | Multiplication: Multiply two operands                                  |   a*b  |
|     /    | Division: Divide first operand from second one                         |   a/b  |
|     %    | Modulus: Divide first operand from second one and return the remainder |   a%b  |
|    **    | Exponent: Raise first operand to the power of second operand           |  a**b  |

---------------------------------
### Truth value
Truth value testing returns `false` only in few scenarios,
1. If the result is `null` or `false`
2. If the result is zero (0, 0.0)
3. Empty sequences and collections ("", '', [], {})
4. If user defined truth value testing method(`$bool$`) returns `false` or `$len$` method returns 0

---------------------------------
### Logical operators
Below are the logical operators:
| Operator |                Description               | Syntax 1 | Syntax 2 |
|:--------:|:----------------------------------------:|:--------:|----------|
|    Or    | Returns true if either operands are true |  a or b  | a \|\| b |
|    And   | Returns true if both operands are true   |  a and b |  a && b  |
|    Not   | Returns true if operand is false         |   not a  |    !a    |

---------------------------------
### Bitwise operators
Below are the bitwise operators:
| Operator |                                        Description                                        | Syntax 1 |
|:--------:|:-----------------------------------------------------------------------------------------:|:--------:|
|     &    | Bitwise AND: Perform bitwise AND between two operands                                     |   a & b  |
|    \|    | Bitwise OR: Perform bitwise OR between two operands                                       |  a \| b  |
|     ^    | Bitwise XOR: Perform bitwise XOR between two operands                                     |   a ^ b  |
|     ~    | Bitwise Complement: Perform bitwise complement on a operand                               |    ~a    |
|    <<    | Left Shift: Shift first operand to the left by number of bits defined in second operand   |  a << b  |
|    >>    | Right Shift: Shift first operand to the right by number of bits defined in second operand |  a >> b  |

---------------------------------

### Comparison operators
Below are the comparison operators:
| Operator |                            Description                           | Syntax |
|:--------:|:----------------------------------------------------------------:|:------:|
|    ==    | True if operands are equal or identical                          | a == b |
|    !=    | True if operands are not equals                                  | a != b |
|     <    | True if left side operand is less than right side                |  a < b |
|     >    | True if left side operand is greater than right side             |  a > b |
|    <=    | True if left side operand is greater than or equal to right side | a <= b |
|    >=    | True if left side operand is less than or equal to right side    | a >= b |
|    in    | True if left side operand is contains in right side operand      | a >= b |

---------------------------------
### Operator precedence
Ordered by decending priority
| Precedence | Associativity |         Operator         |            Description            |
|:----------:|---------------|:------------------------:|:---------------------------------:|
|   Highest  | Left to Right |            ()            | Parenthesis                       |
|      .     | Right to Left |            **            | Exponent                          |
|      .     | Left to Right |             ~            | Complement                        |
|      .     | Left to Right |          +a, -b          | Positive, Negative                |
|      .     | Left to Right |          *, /, %         | Multiplication, Division, Modulus |
|      .     | Left to Right |           +, -           | Addition, Subtraction             |
|      .     | Left to Right |          <<, >>          | Left shift, Right shift           |
|      .     | Left to Right |             &            | Bitwise AND                       |
|      .     | Left to Right |             ^            | Bitwise XOR                       |
|      .     | Left to Right |            \|            | Bitwise OR                        |
|      .     | Left to Right | ==, !=, >, <, >=, <=, in | Comparison operators              |
|      .     | Left to Right |          not, !          | Boolean NOT                       |
|      .     | Left to Right |          and, &&         | Boolean AND                       |
|   Lowest   | Left to Right |         or, \|\|         | Boolean OR                        |

---------------------------------
### Buit-in methods
- `print(value='', end='\n')` : print an object to the standard output stream.
```sh
print("Hello world!")
print("Hello world!", "\n")
print(123, end:" ")
print()
```
- `input(prompt='')` : read a line from the standard input stream. if prompt is provided, it will printed to the stream first.
```sh
name = input("Enter your name: ")
name = input()
```
- `abs(value)` : returns absolute value of an integer or a float.
```sh
value = abs(-10)
```
- `char(value)` : returns character of provided ASCII code.
```sh
c = char(97)
```
- `toInt(value, fromBase=10)` : convert provided value in to an integer.
```sh
dec = toInt("100")
hex = toInt("0xFF1", 16)
bin = toInt("10110", 2)
oct = toInt("723", 8)
```
- `toFloat(value)` : convert provided value in to a floating point value
```sh
f = toFloat("1.7e+3")
```
- `toStr(value)` : convert provided value in to a string
```sh
str = toStr(10)
```
- `split(text, separator)` : break provided text in to sub-strings using separator and returns a list of strings.
```sh
words = split("Hi Hello Sigiri Language", " ")
```
- `len(value)` : returns the count of elements in a object. for user defined classes use `$len$` method to override this.
```sh
count = len([1, 2, 3, 4])
count = len("Hello world!")
```
### Hello world
Hello world program looks like a Python program :D
```sh
print("Hello world!")
```
### Method example
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
### Class example
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

### Inherit example
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

### Default values for arguments
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

### Loading assemblies or libraries
```sh
load system.math
print(math.PI)
print(math.acos(0))
```
`load` keyword can be used to import another Sigiri source or compiled managed library (.dll or .so) in to our program. In above example we are using `math` class from compiled .NET assembly called `system`. (To use a certain library, that library should exists in program location. In above example I have system.dll file and source code in the same directory.)

### Operator overloading
Every operator can be overloaded by providing an method in a class.

| Operator | Description            |             Syntax            |
|:--------:|------------------------|:-----------------------------:|
|     +    | Addition               |      method + (other) { }     |
|     -    | Subtraction            |      method - (other) { }     |
|     *    | Multiplication         |      method * (other) { }     |
|     /    | Division               |      method / (other) { }     |
|     %    | Modulus                |      method % (other) { }     |
|    **    | Exponent               |     method ** (other) { }     |
|     <    | Less than              |      method < (other) { }     |
|     >    | Greater than           |      method > (other) { }     |
|    <=    | Less than or equals    |     method <= (other) { }     |
|    >=    | Greater than or equals |     method >= (other) { }     |
|    ==    | Equals                 |     method == (other) { }     |
|    !=    | Not Equals             |     method != (other) { }     |
|     &    | Bitwise AND            |      method & (other) { }     |
|    \|    | Bitwise OR             |     method \| (other) { }     |
|     ^    | Bitwise XOR            |      method ^ (other) { }     |
|     ~    | Bitwise Complement     |        method ~ () { }        |
|    <<    | Left Shift             |     method << (other) { }     |
|    >>    | Right Shift            |     method >> (other) { }     |
|  and, && | Boolean AND            |     method && (other) { }     |
| or, \|\| | Boolean OR             |    method \|\| (other) { }    |
|  not, !  | Boolean NOT            |        method ! () { }        |
|    in    | Contains               |     method in (other) { }     |
|    []    | Subscript              |     method [] (index) { }     |
|    [=]   | Subscript Assign       | method [=] (index, value) { } |
|    ""    | Convert to String      |        method "" () { }       |
|   $len$  | Get element count      |      method $len$ () { }      |
|  $bool$  | Get as Boolean         |      method $bool$ () { }     |

```sh
class Point {
    method init(x, y) {
        this.x = x
        this.y = y
    }
    method + (other) {
        x = this.x + other.x
        y = this.y + other.y
        return Point(x, y)
    }
    method > (other) {
        if this.x > other.x && this.y > other.y:
            return true
        return false
    }
    method [] (index) {
        if index==0:
            return x
        return y
    }
    method [=] (index, value) {
        if index==0:
            this.x = value
        else:
            this.y = value
    }
    method "" (): return "(" + x + ", "+ y +")"
}
pointA = Point(10, 20)
pointB = Point(25, 42)

pointC = pointA + pointB
print(pointA > pointB)
print(pointC[0])
pointA[1] = 55
print(pointC)

```
