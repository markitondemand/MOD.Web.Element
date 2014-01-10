# MOD.Web.Element

## What?

**Element** is a C# class library built for generating dynamic HTML. It is not a view engine or templating language, it uses normal C# syntax to create “Elements”, which can be manipulated by adding children and/ or setting/ modifying attributes. An “Element” can ultimately be “Rendered” and written out as a string (HTML). When building an HTML page with Element, the most common approach is to hold reference to a container element which may be the &lt;html&gt; node, the &lt;body&gt; node or a container node that is rendered and written to the page once content has been added to it. 


## Why?

There are a myriad of quality approaches to dynamically rendering HTML and all have their attributes. However, **Element** has some unique benefits that have held the test of time, traffic and hundreds of discerning web developers. Below, we’ll outline several of those benefits. But first, a little history. 

The **Element** concept actually started as a JavaScript library. Before jQuery was cool, we wanted a convenient way to dynamically create and manipulate elements in the browser’s DOM. document.createElement was clunky and we were doing quite a bit of on-the-fly element creation in JavaScript. Element.js was our solution to this problem - it allowed us to create nested element structures, apply class names, id’s and other attributes, as well as event handlers in single statements. It was a hit. 

At this time, we were building most of our websites in ASP classic using JScript (don’t hold it against us). As many of us were bemoaning the grim reality of “step-in, step-out” ASP code on very dynamic websites, a lightbulb went off. We could take the same concept and syntax from our JavaScript library and use it in our server-side code. In very little time we were hosting many of our high-profile, high-traffic web sites using the Element approach. Most of us that got in the swing of building sites this way never wanted to look back. 

Several years later, our team made a big transition away from ASP classic to .NET. After evaluating Web Forms and a few other approaches to rendering views, we again decided to carry the **Element** concept on to the next generation. While the syntax changed quite a bit from JavaScript to C#, the basic concept has stayed the same. So why do we like this paradigm so much? A few reasons are outlined below. 

### One Language to Code in

In building our HTML views with Element, we are actually just constructing C# classes. A class can correspond to an HTML document or a “module”. We can also easily write simple methods that create Element structures for us.  We do not have to jump in and out of HTML markup as we do in templates, nor do we have to learn new looping structures. To iteratively add child elements to a parent element, we can simply loop over a collection using a _for_ loop within our C# code. Elements can be passed around and referenced throughout a program and can be programmatically manipulated prior to rendering. 

### Modular

Given that we are building our HTML views in C#, we can create “module” classes that behave 
like partial views, but allow endless flexibility. At it’s simplest, a Module in this paradigm is nothing more than a class that has a container Element. When a module is added as a child to a parent Element, we just add the container Element to that parent. Of course, because our Module is just a C# class, we can leverage concepts like inheritance to increase consistency and reusability. 


### No Spaghetti Code

Stepping in and out of HTML to insert dynamic values inevitably gets messy. There are certainly steps we can take to reduce the mess, but highly dynamic, data-driven sites will become unwieldy in even the most skilled developers hands using a templating approach to rendering HTML. As we’ve noted, using the Element approach, we build our views entirely in C# so there is no stepping in and out. Conditionally adding class names to elements or looping through Model data structures becomes a much more graceful task.

### Easy to Manipulate

Because Elements are objects rather than strings, we can easily do things like add classes to them, modify attributes or append children to them throughout our program. Similarly, we can pass references to these objects between classes as we need. 

### Inheritance in Views

Have modules that share a lot of similarities? Having trouble coding for these similarities in a reusable fashion? Because views (and partial views/ modules) are just C# classes, we can call on inheritance in the true OO sense. We can easily build an abstract base class that defines certain rendering methods, that several descendant classes can reference, increasing code reuse and improving maintainability. 

## How?

Alright, we’ve heard enough talk about **Element**, lets see it in action! Below we’ll go through some examples that show Element’s syntax and some of its more powerful features in action. 

### Syntax

```csharp
//Create a div element
Element container = Element.Create(“div”);

//Create a div with a class name
Element container = Element.Create(“div.container”);

//Create a div with a class name and an id
Element container = Element .Create(“div#myContainer.container”);

//Create an element with some attributes
Element button = Element.Create(“button”, “button-name”, “myButton”, “button-value”, “4”);

//Add children to an element
button.Add(
    Element.Create(“span”).Add(“My Button”)
);

//Create more complex node structures in one statement
Element container = Element.Create(“div.container”).Add(
	Element.Create(“ul”).Add(
		Element.Create(“li”).Add(
			“One”
		),
		Element.Create(“li”).Add(
			“Two”
		),
		Element.Create(“li”).Add(
			“Three”
		)
	)
);

//Methods can return Elements
public static Element RenderButton(string buttonText)
{
	return Element.Create(“button.btn”).Add(
		Element.Create(“span”).Add(buttonText)
	);
}

//And we can modify those elements after the fact
Element button = MyClass.RenderButton(“My Button”);
if(conditional){
	button.AddClass(“super-button”);
}
```

#### Conditionals

Aside from using simple if statements to do things like add classes, we can incorporate logic within nested statements

```csharp
//An example of adding  children conditionally
Element container = Element.Create(“div.container”).Add(
	Element.Create(“h1”).Add(“Title”),
	conditional ? RenderContent() : RenderErrorContent(),
	Element.Create(“div.footer”).Add(
		“Some footer content”
	)
);

//If a null value is added to an element, it is ignored
Element container = Element.Create(“div.container”).Add(
	conditional ? RenderContent() : null
    
);
```


#### Looping

```csharp
//Simple loop example
Element list = Element.Create(“ul”);
for(string name in names)
{
	list.Add(
		Element.Create(“li”).Add(name)
	);
}
```

#### linq

We can take iteration to the next step by using linq statements in our Element statements

```csharp
//Iteratively adding elements using linq
Element list = Element.Create(“ul).Add(
	from item
	in items
	select Element.Create(“li”).Add(item.Name)
);
```

### Tips and Tricks
#### Rendering an Element view in a Controller

Because we use Element with the .Net MVC framework, we need to be able to output actual HTML from our Controllers. Getting the HTML string from an Element is as simple as the following example:

```csharp
Element myElement = Element.Create(“html”).Add(
	//the rest of your page content added here
);
string html = myElement.Render().ToString();
```

In this scenario, we will also want to output the doctype before we write out HTML, so we can create the following helper method that internally calls System.Web.MVC.Controllers Content method:

```csharp
public ActionResult Content(IRenderable m)
{
	string docType = @"<!DOCTYPE html>";
	Response.Write(docType );
	return Content(m.Render().ToString());
}
```

Finally, our controller actions will look something like the following:

```csharp
public ActionResult Index()
{
	Element myElement = Element.Create(“html”).Add(
		//the rest of your page content added here
	);
	return Content(myElement);
}
```

*note that Element structures generally should not be created within Controllers, the above is simply for the sake of example.
