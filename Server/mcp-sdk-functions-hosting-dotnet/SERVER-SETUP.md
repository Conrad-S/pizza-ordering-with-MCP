
Title:  Host remote MCP servers built with official MCP SDKs on Azure Functions
URL:    https://github.com/Azure-Samples/mcp-sdk-functions-hosting-dotnet?tab=readme-ov-file#prerequisites


Notes:
 - This is a modified version of the sample above. The main modification is the addition of pizza ordering
   in addition to the weather functionality.

PREREQUISITES
-------------
 - Review the link to the original repo for prerequisites (see the URL above).
 - Next, Clone this repo. DO NOT clone the original repo (do not clone from the URL above).
 
 - I suggest running the MCP server (function app) locally to start. 
 - Once you are comfortable you can deploy to Azure.

TO START
--------
To start the server (two distinct steps):
 1. In Terminal (Bash):
    - Go to the folder FOUNDRY-AGENT-AND-MCP\Server\mcp-sdk-functions-hosting-dotnet
    - type: func start (starts the Azure Function in Visual Studio Code)

 2. In Files list on the left side of the Visual Studio Code window, open the file: .vscode\mcp.json
    - Validate: 
        - Validate that the localhost port is correct (this is the port displayed when starting the server in step 1).
        - If there is no entry at all, press the Add Server button at the bottom-right of the mcp.json file (requires the MCP extension),
          and add the server: add by URL, use http://localhost:7071/mcp, it will prompt you for a friendly name- select the default- don't skip this step.

          The entry in mcp.json should look something like this:

                  {
            [Error][Restart] <-- these are buttons at the top.
            "servers": {
                "local-mcp-server": {
                    "url": "http://localhost:7071/mcp",
                    "type": "http"
                },
            ...

    - Action
        - Look at the top of the first entry for "Start" button. Press it. It should look like this:
        - If the buttons are missing, confirm that the MCP Extension is installed in Visual Studio Code.
        - If you cannot determine the issue, restart Visual Studio.

    - Note!: Do NOT browse to the MCP URL (for example, "http://localhost:7071/mcp"). Nothing is visible here.

To ask a question:
1. Once the server is started, open a NEW GitHub Copilot chat.
2. Verify that GitHub Copilot chat is set to Agent mode.

    Type the following questions and press the submit button in GitHub Copilot chat:
    - How is the weather today in New York?
    - Can I please see the pizza menu?

GitHub Copilot chat should prompt to allow use of a MCP server, and should be able to answer both questions.