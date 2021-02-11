# Strings
Strings are string objects in Sigiri. `system.string` library provides some constants associated with strings.

### How to define a string?
Texts surrounding by double quotes or single quotes are known as a string.
```sh
str = "this is a string"
str = 'this is also a string'
str = "this is
a \"multiline\"
string"
```

### Iterable
Strings are iterable objects.
```sh
str = "Sigiri string"
for each item in str:
	print(item,end:" ")
```

### Subscriptable
You can access and modify individual characters in a string using square brackets. (zero based indexing)
```sh
str = "my string"
print(str[0]) 	// output: m
str[0] = 'M' 	// changes string to "My string"
```

### Attributes of a string object
Every string object will have following read-only attributes. you can access them as normal attribute. For example,
```sh
str = "Sigiri"
str.length
```
- `length` : gives the length of the string


### Associated methods
For following associated methods you don't need any libraries. Those are built-in methods for string objects. Example,
- `append(value*)` : Return new string with value(s) appended at the end of string.
```sh
str = "Sig"
str = str.append("iri")	// now str is "Sigiri"
```
- `capitalize()` : Return new string with first character in uper-case.
```sh
str = "this is a string"
str = str.capitalize()	// now str is "This is a string"
```
- `clone()` : Return new string object with contents of current object.
```sh
str = "this is a string"
myClone = str.clone()
```
- `center(width, fillChar=' ')` : Return new center aligned string.
```sh
str = "HELLO"
str2 = str.center(7) // now str2 is " HELLO "
str3 = str.center(11, '=') // now str3 is "===HELLO==="
```
- `count(str, start=0, count=null)` : Return the number of occurrences of substring.
```sh
str = "Sigiri"
count = str.count('i') // now count is 3
count2 = str.count('i', 2) // now count2 is 2
count3 = str.count('ri', 0, 3) // now count3 is -1 (not found "ri" in given range)
```
- `encode(encoding='utf8')` : Return byte array object of current string. Supported encoding types: ascii, utf8, utf7, utf32, unicode and bigendian.
```sh
str = "abc"
e1 = str.encode() // now e1 is bytes[0x61, 0x62, 0x63]
e2 = str.encode("ascii") // now e1 is bytes[0x61, 0x62, 0x63]
```
- `endsWith(str)` : Return true if the current string ends with specified substring.
```sh
str = "Sigiri"
val = str.endsWith('ri') // now val is true
val = str.endsWith('a') // now val is false
```
- `insert(value, index)` : Return new string with specified value inserted at the specified index.
```sh
str = "Siri"
str = str.insert("gi", 2) // now str is "Sigiri"
```
- `index(str, start=0, count=null)` : Return the index of first occurance of the specified substring.
```sh
str = "Sigiri"
pos = str.index("i") // now pos is 1
pos = str.index("i", 2) // now pos is 3
pos = str.index("i", 4, 1) // now pos is 5
```
- `isDigits(baseValue)` : Return true if all characters of string is a digit of specified base number system.
```sh
str = "127"
val = str.isDigits() // now val is true
val = str.isDigits(2) // now val is false
val = str.isDigits(8) // now val is true
val = str.isDigits(16) // now val is true
```
- `lastIndex(str, start=0, count=null)` : Return the index of last occurance of the specified substring.
```sh
str = "Sigiri"
pos = str.lastIndex("i") // now pos is 5
pos = str.lastIndex("i", 4) // now pos is 3
pos = str.lastIndex("i", 2, 2) // now pos is 1
```
- `padLeft(width, fillChar=' ')` : Return new right aligned string.
```sh
str = "HELLO"
str2 = str.padLeft(7) // now str2 is "  HELLO"
str3 = str.padLeft(8, '-') // now str3 is "---HELLO"
```
- `padRight(width, fillChar=' ')` : Return new left aligned string.
```sh
str = "HELLO"
str2 = str.padRight(7) // now str2 is "HELLO  "
str3 = str.padRight(8, '-') // now str3 is "HELLO---"
```
- `replace(old, new, ignoreCase=false)` : Return new string with all the occurances of old value replaced with new value.
```sh
str = "Abz Abz"
str2 = str.replace("Ab", "xy") // now str2 is "xyz xyz"
str3 = str.replace("ab", "xy", true) // now str2 is "xyz xyz"
```
- `reverse()` : Return new string with reversed order of current string.
```sh
str = "Sigiri"
str = str.reverse() // now str is "irigiS"
```
- `subStr(start, length)` : Return substring with specified length at starting index. 
```sh
str = "Sigiri Language"
str2 = str.subStr(7) // now str2 is "Language"
str3 = str.subStr(0, 5) // now str3 is "Sigiri"
```
- `split(separators, count)` : Return list object of substrings sliced by the separator. This method accepts a string or list of strings for separators.
```sh
str = "a,b,c.d*e*f"
lst1 = str.split(",") // now lst1 is ['a', 'b', 'c.d*e*f']
lst2 = str.split([',', '.', '*']) // now lst2 is ['a', 'b', 'c', 'd', 'e', 'f']
```
- `toLower()` : Return new string with all characters in lower-case.
```sh
str = "Sri Lanka"
str = str.toLower() // now str is "sri lanka"
```
- `toUpper()` : Return new string with all characters in upper-case.
```sh
str = "Sri Lanka"
str = str.toUpper() // now str is "SRI LANKA"
```
- `trim()` : Return new string without leading and tailing white spaces.
```sh
str = "  Sigiri Language "
str = str.trim() // now str is "Sigiri Language"
```
- `trimEnd()` : Return new string without tailing white spaces.
```sh
str = "  Sigiri Language "
str = str.trimEnd() // now str is "  Sigiri Language"
```
- `trimStart()` : Return new string without leading white spaces.
```sh
str = "  Sigiri Language "
str = str.trimStart() // now str is "Sigiri Language "
```
- More coming soon

### String library
`system.string` library provides few constant string values.
- `asciiLetters` : All ascii letters ('abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ')
- `asciiLower` : All lower-case ascii letters ('abcdefghijklmnopqrstuvwxyz')
- `asciiUper` : All upper-case ascii letters ('ABCDEFGHIJKLMNOPQRSTUVWXYZ')
- `digits` : 0-9 digits ('0123456789')
- `octdigits` : oct letters ('01234567')
- `hexdigits` : hex letters ('0123456789abcdefABCDEF')
- `punctuation` : Punctuation characters.

### Notes
- String concadination can be done using `+` operator.
```sh
str = "Sigiri " + "Language"
```
- Strings can be multiplied with integers.
```sh
str = "Sigiri " * 3 // now str is "Sigiri Sigiri Sigiri"
```
- Built-in `len(value)` method can be used to get the length of a string.
