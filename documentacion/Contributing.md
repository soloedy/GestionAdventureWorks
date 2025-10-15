# Contribution Guide

## 1. Method, Function, and Variable Naming

- **Method and function names**: Use the function type prefix (`Fnc`, `Sub`, etc.) followed by the name in `PascalCase`.
- **Variable names**: Use the variable type prefix (`Int`, `Bln`, etc.) followed by the name in `PascalCase`.

### Prefix Table for Variables

| Data Type      | Prefix | Example Variable Name   |
|----------------|--------|------------------------|
| Integer        | Int    | IntMonth               |
| Boolean        | Bln    | BlnIsActive            |
| String         | Str    | StrProductName         |
| Double         | Dbl    | DblPrice               |
| Decimal        | Dec    | DecBalance             |
| DateTime       | Dtm    | DtmBirthDate           |
| Object         | Obj    | ObjClient              |
| List           | Lst    | LstProducts            |
| Array          | Arr    | ArrValues              |
| Char           | Chr    | ChrInitial             |
| Byte           | Byt    | BytData                |
| DataTable      | Dtb    | DtbResults             |
| DataRow        | Drw    | DrwRow                 |
| DataSet        | Dts    | DtsData                |
| Form           | Frm    | FrmMain                |

> **Note:** These prefixes are suggested. You can adapt them according to your project's needs or add other specific types you use.

### Naming Classes, Interfaces, and Other Components

- **Classes**: Use `PascalCase` and descriptive names that represent the purpose of the class.
- **Interfaces**: Use the prefix `I` followed by a name in `PascalCase`.
- **Public properties and methods**: Use `PascalCase`.
- **Razor files (.cshtml)**: Use `PascalCase` for file names and be descriptive.
- **Razor Components (Blazor)**: Use `PascalCase` and end the name with "Component" when appropriate.

## 3. Code Comments

- **Comment the why, not just the what**: Explain the reason behind complex decisions, not just what the code does if it's already evident from the function or variable name.
- **Avoid obvious comments**: Do not comment on what is already clear from the context or clear names.
- **Update or remove obsolete comments**: An outdated comment can cause more confusion than help.
- **Prefer comments in Spanish** to maintain consistency, unless the team decides otherwise.

### Types of Comments

- **Line comment**: Use `//` for brief comments about a specific line or block.
- **Block comment**: Use `/* ... */` for more extensive explanations or temporary comments.
- **XML documentation comments**: For public methods, classes, and properties, use the XML documentation format (`///`). This facilitates automatic documentation generation and helps with IntelliSense in Visual Studio.
- **In Razor (.cshtml)**: Use `@* ... *@` for comments in Razor files.

## 4. Error Handling

- **Use specific exceptions**: Whenever possible, throw and catch specific exceptions instead of the generic `Exception`.
- **Avoid catching exceptions without processing them**: Do not use empty catch blocks or ignore errors.
- **Do not expose internal details**: When displaying error messages to the user, use clear messages but never show sensitive information (call stack, table names, etc.).
- **Error handling in Razor**: Use `try-catch` blocks in the backend and display user-friendly error messages in the interface.
- **Use pre-validations**: Before executing operations prone to failure (such as file or database access), validate inputs and conditions to avoid unnecessary exceptions.

> **Remember:** Good error handling improves user experience and facilitates system maintenance and debugging.

## 5. Error Handling and Best Practices in SQL

- **Error control**: Use `TRY...CATCH` blocks to capture and handle errors in stored procedures or complex scripts.
- **Avoid displaying sensitive details**: When handling errors, do not expose confidential information in error messages visible to users.
- **Pre-validations**: Before performing critical operations (DELETE, UPDATE, INSERT), validate the existence and state of the data.
- **Clear comments**: Use `--` for single-line comments and `/* ... */` for more extensive blocks.
- **Secure SQL**: Use parameters to prevent SQL injection.
	
## 6. structure of an api response

{
  "Status": true,                // true si la operación fue exitosa, false si hubo error
  "Data": { ... },               // objeto con los datos devueltos (solo si Status es true)
  "Message": "Texto amigable",   // mensaje claro sobre el resultado (éxito o error)
  "Observacion": "Detalle técnico del error (si aplica)" // solo si hubo error, puede ser null en éxito
}