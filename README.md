# AppDevGame

## Prerequisites

Before you can build and run the project, ensure you have the following installed on your system:

- [Visual Studio Code](https://code.visualstudio.com/)
- [MonoGame Framework](https://www.monogame.net/downloads/)
- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Visual Studio](https://visualstudio.microsoft.com/) (Optional, if you prefer using Visual Studio)

## Getting Started

## Cloning the Repository

Clone the repository to your local machine using the following command:
gh repo clone andriichumakov/AppDevGame

## OR

git clone https://github.com/andriichumakov/AppDevGame.git
cd AppDevGame

## Unblocking .NET Tools

After cloning the repository, you need to unblock the dotnet-tools.json file:

1. Navigate to the .config folder.
2. Right-click on the dotnet-tools.json file and select Properties.
3. In the Properties window, check the Unblock checkbox and apply the changes.

## Restoring .NET Tools
Open a terminal in the project directory and run the following command to restore .NET tools:

```
dotnet restore
```

## Building the Solution
Run the following command to build the project:

```
dotnet build
```
Ensure that there are no build errors.

## Running the Project
Run the following command to start the project:

```
dotnet run
```

If the program does not run, try reopening the project in Visual Studio Code and running `dotnet run` again.

## In order to run the project in Visual studio, do the following steps

Open the Project in Visual Studio:

    Launch Visual Studio.
    Open the solution file (AppDevGame.sln) located in the root directory of the cloned repository.

Install MonoGame Extension:

    If you haven't already, install the MonoGame extension for Visual Studio. This can be found in the Visual Studio Marketplace.

Build the Solution:

    Select Build > Build Solution from the top menu.
    Ensure that there are no build errors.

Run the Project:

    Press F5 or click the Start button in Visual Studio to build and run the project.
    The game window should launch, allowing you to play the game.

## In order to run the project in VSCode, do the following steps

Open the Project in VSCode:

    Launch Visual Studio Code.
    Open the folder containing the cloned repository.

Install C# Extensions:

    If you haven't already, install the C# extension for VSCode by Microsoft. This can be found in the Extensions view (Ctrl+Shift+X).

Restore Project Dependencies:

    Open a terminal in VSCode (`Ctrl+``) and run the following command to restore the project dependencies:

```
dotnet restore
```

Build the Solution:

    In the terminal, run the following command to build the project:

```
dotnet build
```

Ensure that there are no build errors.

Run the Project:

    Press F5 to start debugging the project. This will build and run the project.
    Alternatively, you can run the project using the terminal:

```
dotnet run
```
