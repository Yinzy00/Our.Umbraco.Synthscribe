# Umbraco Synthscribe

*An AI-powered content creation tool for Umbraco.*

- [Installation](#installation)
- [Usage](#usage)
- [License](#license)
- [Acknowledgements](#Acknowledgements)

<br/>

## Installation
1. Install the NuGet package:

```bash
Nuget command here
```

2. Add your OpenAi api key to the appsettings.json file:

```json
"OpenAi": {
    "apiKey": "KEY HERE"
  }
```

3. Done, the package has been installed !

<br/>

## Usage
### Generate text content using context

<br/>

For generating you go in to the content section of the backoffice. Select the textfield you want te content to be generated in and type the command.

<br/>

**Generate content using context using two steps:**

1. Start with:

```
{?
```

2. End with:

```
;}
```

3. Done! This will generate text!

<br/>

**Example:**

```
{?Umbraco 10 being the most user friendly CMS in the world;}
```

After closing the command with the last ‘}’ the content of the field will be replaced with the AI generated content (may take some time depending on how much content is being generated).

<br/>

You can add **parameters** to it to push the ai in the right direction.

```
{?Umbraco 10 being the most user friendly CMS in the world (max 3 paragraphs);}
```

Or 

```
{?Umbraco 10 being the most user friendly CMS in the world (max 200 chars);}
```

<br/>

### Generate dummy data

Use the command “rnd”.

```
{?rnd;}
```

 This will generate lorem ipsum data according to the type of field you are in.

<br/>

## License
Copyright &copy; [Yari Mariën](https://github.com/Yinzy00/).

All source code is licensed under the [Mozilla Public License 2.0](../LICENSE).

For more information about the Mozilla Public License 2.0, please visit: https://www.mozilla.org/en-US/MPL/2.0/FAQ/

<br/>

## Acknowledgements

**Developers**
- [Yari Mariën](https://github.com/Yinzy00/)
