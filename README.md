# Pizza Ordering with MCP

This is a modified version of a GitHub sample that deploys an MCP server as an Azure Function. 
The main modification is the addition of pizza ordering in addition to the weather functionality.

Original URL (do not clone this URL): https://github.com/Azure-Samples/mcp-sdk-functions-hosting-dotnet

Do not clone the original URL, instead, clone this repo.

## Repository Structure
This GitHub repo modifies the original sample to create a client folder and server folder, each with MCP samples. The structure:

```
├── README.md                              # This file
├── foundry-agent-and-mcp.sln              # Visual Studio solution file (used by the server, the file is here since .sln files are one level above the project file.
├── Client/                                # Client: sample code to call an existing MCP server.
│   └── mcp.ipynb                          # C-sharp notebook with sample code to call an existing MCP Server
└── Server/                                # Server folder (holds the server project)
    └── mcp-sdk-functions-hosting-dotnet/  # Pizza ordering MCP server
        ├── Tools/
        │   ├── WeatherTools.cs            # weather forcast MCP (existed in the original sample).
        │   └── PizzaTools.cs              # Pizza ordering tool for MCP (added to this sample).
        └── ...
```

# - Client -
The client is a C# notebook that calls an existing (sample) MCP server. Please see the instructions in the C# notebook for details.

# - Server -

## Prerequisites

- Review the link to the original repo for prerequisites (see the URL above).
- Next, Clone this repo. DO NOT clone the original repo (do not clone from the Original URL above).
- Suggest running the MCP server (function app) locally in Visual Studio Code to start.
- Once you are comfortable, you can deploy to Azure.

## To Start the MCP server and test:

To start the server (two distinct steps):

### 1. In Terminal (Bash):
- Go to the folder `\Server\mcp-sdk-functions-hosting-dotnet`
- Type: `func start` (starts the Azure Function in Visual Studio Code)

### 2. In Files list on the left side of the Visual Studio Code window, open the file: `.vscode\mcp.json`

**Validate:**
- Validate that the localhost port is correct (this is the port displayed when starting the server in step 1).
- If there is no entry at all, press the Add Server button at the bottom-right of the mcp.json file (requires the MCP extension), and add the server: add by URL, use `http://localhost:7071/mcp`, it will prompt you for a friendly name- select the default- don't skip this step.

The entry in mcp.json should look something like this:

```json
{
  "servers": {
    "local-mcp-server": {
      "url": "http://localhost:7071/mcp",
      "type": "http"
    },
    // ...
  }
}
```

**Action:**
- Look at the top of the first entry for "Start" button. Press it.
- If the buttons are missing, confirm that the MCP Extension is installed in Visual Studio Code.
- If you cannot determine the issue, restart Visual Studio.

**Note!**: Do NOT browse to the MCP URL (for example, "http://localhost:7071/mcp"). Nothing is visible here.

## To ask a question:

1. Once the server is started, open a NEW GitHub Copilot chat.
2. Verify that GitHub Copilot chat is set to Agent mode.

Type the following questions individually and press the submit button in GitHub Copilot chat after asking each question:
- How is the weather today in New York?
- Can I please see the pizza menu?

GitHub Copilot chat should prompt to allow use of an MCP server, and should be able to answer both questions.

## Deployment

This project includes Azure infrastructure as code. To deploy to Azure:

```bash
cd Server/mcp-sdk-functions-hosting-dotnet
azd up
```

## License

This project is based on Microsoft's MCP SDK sample and maintains the original MIT License. See `Server/mcp-sdk-functions-hosting-dotnet/LICENSE.md` for details.

## Attribution

Based on Microsoft's [MCP SDK Functions Hosting sample](https://github.com/Azure-Samples/mcp-sdk-functions-hosting-dotnet) with pizza ordering functionality added.
