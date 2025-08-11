# SARSCustomsRiskEngine Documentation

## How To Run Application

Download Visual Studio Community Edition: https://visualstudio.microsoft.com/vs/community/  
<br/>Install the C# Console package from the installer.

Clone the project

```
git clone https://github.com/Sabelo710/SARSCustomsRiskEngine.git
```

Enter th**e SARSCustomsRiskEngine** folder

```bash
cd SARSCustomsRiskEngine
```

Open the Visual Studio solution's file: **SARSCustomsRiskEngine.sln (should be done after successfully installing Visual Studio above)  
<br/>Start the application  
<br/>**
<img width="1852" height="682" alt="1-c#-start-app" src="https://github.com/user-attachments/assets/c8fa79ac-9588-4603-8ce4-d09d9e1171e4" />
<img width="1487" height="762" alt="2-c#-app-running" src="https://github.com/user-attachments/assets/28dce66a-d92a-4073-9406-3518ee45689b" />


# Architecture Overview

1.  **Layered & Plug-in Architecture**  
    The code is split into **thin horizontal layers** that only depend downward, enabling independent evolution and testability.
    
    Copy
    
    ```
    ┌────────────┐  CLI / UI / Web API (thin, replaceable)
    │  Program   │
    ├────────────┤
    │  Results   │  DTO returned to callers (RpnParseResult)
    ├────────────┤
    │  Parsers   │  Tokenizer + AST builder (RpnParser)
    ├────────────┤
    │   Nodes    │  Concrete AST nodes (OperandNode, OperatorNode)
    ├────────────┤
    │Abstractions│  Contracts (ExpressionNode)
    ├────────────┤
    │  Models    │  Immutable value objects (Operand, Operator)
    └────────────┘
    ```
    
2.  **Domain-Driven Building Blocks**
    
    - **Value Objects** – `Operand`, `Operator` encapsulate scalar values & behavior.
        
    - **Entities / Nodes** – `OperandNode`, `OperatorNode` form a small *Expression Tree* aggregate.
        
    - **Domain Service** – `RpnParser` orchestrates tokenization and tree construction.
        
    - **Anti-Corruption Layer** – `RpnParseException` prevents leaking low-level parsing errors.
        
3.  **Open/Closed Design**
    
    - New operators: extend `Operator.cs` only.
        
    - New syntax (e.g., infix): add another parser class; reuse existing nodes & models.
        
4.  **Testability**
    
    - Every public type is small, sealed, and has single responsibility → trivial unit tests.
        
    - No static state; parser is instantiable and side-effect free.
        
5.  **Deployment Flexibility**
    
    - Core assembly (`*.Core`) can be packaged as NuGet and reused in web APIs, Azure Functions, or Blazor apps.
        
    - CLI (`Program.cs`) is a thin adapter that can be swapped for any other host.
        

In short, the architecture is a **micro-domain layer** with clean boundaries, minimal surface, and maximal extensibility.
