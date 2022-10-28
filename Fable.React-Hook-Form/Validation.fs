namespace Fable.ReactHookForm

open System.Text.RegularExpressions
open Fable.Core.JS

module Validation =
    type IntRule = { value: int; message: string }
    type DecimalRule = { value: decimal; message: string }
    type PatternRule = { value: Regex; message: string }
    type CustomValidateRule<'T> = 'T -> Result<'T,string>
    type CustomValidateRule'<'T> = 'T -> string option
    type CustomValidateAsyncRule<'T> = 'T -> Async<Result<'T,string>>
    type CustomValidatePromiseRule<'T> = 'T -> Promise<string option>

    type Rule<'T> =
        | Required of string

        | MaxLength of int * string
        | [<CompiledName("maxLength")>] MaxLength' of IntRule

        | MinLength of int * string
        | [<CompiledName("maxLength")>] MinLength' of IntRule

        | Max of decimal * string
        | [<CompiledName("max")>] Max' of DecimalRule

        | Min of decimal * string
        | [<CompiledName("mmin")>] Min' of DecimalRule

        | Pattern of string * string
        | [<CompiledName("pattern")>] Pattern' of PatternRule

        | Validate of CustomValidateRule<'T>
        | [<CompiledName("validate")>] Validate' of CustomValidateRule'<'T>
        | [<CompiledName("validate")>] ValidateAsync of CustomValidateAsyncRule<'T>
        | [<CompiledName("validate")>] ValidatePromise of CustomValidatePromiseRule<'T>

    type ValidationError = { message: string }