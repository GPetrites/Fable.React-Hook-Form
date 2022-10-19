namespace Fable.ReactHookForm

open System.Text.RegularExpressions
open Fable.Core.JS

module Validation =
    type IntRule = { value: int; message: string }
    type DecimalRule = { value: decimal; message: string }
    type PatternRule = { value: Regex; message: string }
    type CustomValidateRule = string -> string option
    type CustomValidateAsyncRule = string -> Async<string option>
    type CustomValidatePromiseRule = string -> Promise<string option>

    type Rule =
        | Required of string
        | MaxLength of IntRule
        | MinLength of IntRule
        | Max of DecimalRule
        | Min of DecimalRule
        | Pattern of PatternRule
        | Validate of CustomValidateRule
        | [<CompiledName("validate")>] ValidateAsync of CustomValidateAsyncRule
        | [<CompiledName("validate")>] ValidatePromise of CustomValidatePromiseRule

    type ValidationError = { message: string }