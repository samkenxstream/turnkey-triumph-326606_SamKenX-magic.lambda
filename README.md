
# Magic Lambda for .Net

[![Build status](https://travis-ci.org/polterguy/magic.lambda.svg?master)](https://travis-ci.org/polterguy/magic.lambda)

Magic Lambda is a microscopic Turing complete programming language based upon [Magic Node](https://github.com/polterguy/magic.node)
and [Magic Signals](https://github.com/polterguy/magic.signals). It provides the familiar _"keywords"_, such as **[for-each]**
and **[if]**, through creating Super Signal Slots for these keywords, making them easily available for you through your Hyperlambda code.

This allows you to dynamically invoke C# methods, from Hyperlambda code, making your C# much more dynamic in nature, while
allowing you to easily extend the programming language yourself, by creating your own slots for it.

## Supported keywords

* __[case]__
* __[default]__
* __[else]__
* __[else-if]__
* __[if]__
* __[switch]__
* __[add]__
* __[apply-file]__
* __[convert]__
* __[insert-after]__
* __[insert-before]__
* __[remove-node]__
* __[set-name]__
* __[set-value]__
* __[unwrap]__
* __[eq]__
* __[exists]__
* __[lt]__
* __[lte]__
* __[mt]__
* __[mte]__
* __[throw]__
* __[try]__
* __[.catch]__
* __[.finally]__
* __[and]__
* __[not]__
* __[or]__
* __[for-each]__
* __[while]__
* __[get-count]__
* __[get-name]__
* __[get-nodes]__
* __[get-value]__
* __[concat]__
* __[contains]__
* __[regex-replace]__
* __[replace]__
* __[starts-with]__
* __[to-lower]__
* __[to-upper]__
* __[eval]__
* __[vocabulary]__

The above are all implemented as `ISlots`, accessible to you through raising a signal, using the `ISignal` provider from Magic Signals.

## License

Although most of Magic's source code is publicly available, Magic is _not_ Open Source or Free Software.
You have to obtain a valid license key to install it in production, and I normally charge a fee for such a
key. You can [obtain a license key here](https://servergardens.com/buy/).
Notice, 5 hours after you put Magic into production, it will stop functioning, unless you have a valid
license for it.

* [Get licensed](https://servergardens.com/buy/)
