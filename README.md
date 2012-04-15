SLAP!
---------------

A free command line tool for interacting with online paste sites.

  - Uploads code to the KDE Paste
  - Automatically copies the links to your clipboard

Slap is written in C# and runs in .NET as well as Mono, allowing for use in Windows, Mac, Linux, and any OS where Mono can run!

For more details check the [website]

  [website]: http://crowell.github.com/slap/main.html
  
# Slap

## What is Slap?

Slap is a command line tool for uploading files to the KDE paste service
It uploads code to the KDE paste and automatically copies the links to your clipboard.

## Features

* Supports KDE Paste's Api including
* Paste Language
* Privacy
* Deletion timer
* Support for .NET and Mono
* and _much_ more (to come)!

## Installation

Requires .NET runtime on Windows.  Requires Mono on Mac or Linux.

## Installation for Development

If you want to develop, the solution can be opened in Visual Studio or Monodevelop.


## Testing it out

run the executable as is

slap.exe </path/to/file> 

then check your pasteboard!

That's it!

## More Information

* Follow [endtwist](http://twitter.com/jeffreycrowell) on twitter for updates

## Compatibility

Currenly the debug executable is built for .NET 4.0 compatibility

## Contributing

Pull requests are being accepted! If you would like to contribute, simply fork
the project and make your changes.

### Style Guide

If you intend on contributing, please follow this style guide when submitting
patches or commits. Submissions that do not follow these guidelines will not
be accepted.

* Blank line at the end of files
* Semi-colons at the ends of lines, where appropriate
* Keep lines to 80 characters or less
* Never bump the version
* ANSI code style (feel free to use astyle)

No whitespace between function name and args:
    void foo(bar)
    // good
    
    void foo (bar)
    // bad


Ternary expressions are usually not fine, please use if/else if/else blocks instead unless absolutely necessacary:


Methods:
    foo.bar = function() 
    {
    }
    // good
    
    foo.bar = function (){
    }
    // bad
    
    foo.bar = function(){
    }
    // bad
