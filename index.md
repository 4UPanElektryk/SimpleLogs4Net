## Welcome to GitHub Pages

You can use the [editor on GitHub](https://github.com/4UPanElektryk/SimpleLogs4Net/edit/gh-pages/index.md) to maintain and preview the content for your website in Markdown files.

Whenever you commit to this repository, GitHub Pages will run [Jekyll](https://jekyllrb.com/) to rebuild the pages in your site, from the content in your Markdown files.

### Markdown

Markdown is a lightweight and easy-to-use syntax for styling your writing. It includes conventions for

```markdown
Syntax highlighted code block

# Header 1
## Header 2
### Header 3

- Bulleted
- List

1. Numbered
2. List

**Bold** and _Italic_ and `Code` text

[Link](url) and ![Image](src)
```

# Usage
First you need to add Initialize 
```cs
Log log = new Log("Directory where you want to store logs");
```
Example of Usage
```cs
using System;

namespace SimpleLogs4Net.Tests
{
    internal class Program
    {
        static Log log;
        static void Main(string[] args)
        {
            log = new Log(AppDomain.CurrentDomain.BaseDirectory + "Logs\\");
            Log.AddEvent(new Event("text", Event.Type.Normal));
            Log.AddEvent(new Event("text", Event.Type.Informtion));
            Log.AddEvent(new Event("text", Event.Type.Warrning));
            Log.AddEvent(new Event("text", Event.Type.Error));
            Log.AddEvent(new Event("text", Event.Type.Critical_Error));
        }
    }
}
```
