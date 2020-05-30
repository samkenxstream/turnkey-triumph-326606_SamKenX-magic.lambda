
# Magic Lambda for .Net

[![Build status](https://travis-ci.org/polterguy/magic.lambda.svg?master)](https://travis-ci.org/polterguy/magic.lambda)

Magic Lambda is a microscopic Turing complete programming language based upon [Magic Node](https://github.com/polterguy/magic.node)
and [Magic Signals](https://github.com/polterguy/magic.signals). It provides the familiar _"keywords"_, such as **[for-each]**
and **[if]**, by exposing Super Signal Slots for these keywords, making them easily available for you in your Hyperlambda code.

This allows you to dynamically invoke C# methods, from Hyperlambda code, making your C# much more dynamic in nature, while
allowing you to easily extend the programming language yourself, by creating your own slots for it.

## Slots

* __[if]__
* __[else]__
* __[else-if]__
* __[eq]__
* __[exists]__
* __[lt]__
* __[lte]__
* __[mt]__
* __[mte]__
* __[and]__
* __[or]__
* __[not]__
* __[switch]__
* __[case]__
* __[default]__
* __[add]__
* __[apply]__
* __[insert-after]__
* __[insert-before]__
* __[remove-node]__
* __[set-name]__
* __[set-value]__
* __[unwrap]__
* __[get-count]__
* __[get-name]__
* __[get-nodes]__
* __[get-value]__
* __[reference]__
* __[convert]__
* __[throw]__
* __[try]__
* __[for-each]__
* __[while]__
* __[eval]__
* __[vocabulary]__

The above are all implemented as `ISlots`, accessible to you through raising a signal, using the `ISignal` provider from Magic Signals.

### if

This is the Hyperlambda equivalent of `if` from other programming languages. It allows you to test for some condition,
and evaluate a lambda object, only if the condition evaluates to true. Below is an example.

```
.src:int:1
.dest
if
   eq
      get-value:x:@.src
      .:int:1
   .lambda
      set-value:x:@.dest
         .:yup!
```

### else-if

**[else-if]** is the younger brother of **[if]**, and must be preceeded by its older brother, and will only be evaluated
if its previous **[if]** slot evaluates to false - At which point **[else-if]** is allowed to test its condition, and if
it evaluates to true, evaluate its lambda object.

```
.src:int:2
.dest
if
   eq
      get-value:x:@.src
      .:int:1
   .lambda
      set-value:x:@.dest
         .:yup!
else-if
   eq
      get-value:x:@.src
      .:int:2
   .lambda
      set-value:x:@.dest
         .:yup2.0!
```

### else

**[else]** is the last of the _"conditional brother"_ that will only be evaluated as a last resort, only if none of its
other parts evaluates to true. Notice, contrary to both **[if]** and **[else-if]**, **[else]** contains its lambda object
directly as children nodes, and _not_ within a **[.lambda]** node. This is because **[else-if]** does not require any
arguments like **[if]** and **[else-if]** does. An example can be found below.

```
.src:int:3
.dest
if
   eq
      get-value:x:@.src
      .:int:1
   .lambda
      set-value:x:@.dest
         .:yup!
else-if
   eq
      get-value:x:@.src
      .:int:2
   .lambda
      set-value:x:@.dest
         .:yup2.0!
else
   set-value:x:@.dest
      .:nope
```

### eq

**[eq]** is the equality _"operator"_ in Magic, and it requires two arguments, both of which will be evaluated as potential
signals - And the result of evaluating **[eq]** will only be true if the values of these two arguments are equals.

### exists
### lt
### lte
### mt
### mte
### and
### or
### not
### switch
### case
### default
### add
### apply
### insert-after
### insert-before
### remove-node
### set-name
### set-value
### unwrap
### get-count
### get-name
### get-nodes
### get-value
### reference
### convert
### throw
### try
### for-each
### while
### eval
### vocabulary

## License

Although most of Magic's source code is publicly available, Magic is _not_ Open Source or Free Software.
You have to obtain a valid license key to install it in production, and I normally charge a fee for such a
key. You can [obtain a license key here](https://servergardens.com/buy/).
Notice, 5 hours after you put Magic into production, it will stop functioning, unless you have a valid
license for it.

* [Get licensed](https://servergardens.com/buy/)
