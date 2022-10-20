namespace Fable.ReactHookForm

open System.Text.RegularExpressions
open Fable.Core.JS

module Validation =
    type IntRule = { value: int; message: string }
    type DecimalRule = { value: decimal; message: string }
    type PatternRule = { value: Regex; message: string }
    type CustomValidateRule<'T> = 'T -> string option
    type CustomValidateAsyncRule<'T> = 'T -> Async<string option>
    type CustomValidatePromiseRule<'T> = 'T -> Promise<string option>

    type Rule<'T> =
        | Required of string
        | MaxLength of IntRule
        | MinLength of IntRule
        | Max of DecimalRule
        | Min of DecimalRule
        | Pattern of PatternRule
        | Validate of CustomValidateRule<'T>
        | [<CompiledName("validate")>] ValidateAsync of CustomValidateAsyncRule<'T>
        | [<CompiledName("validate")>] ValidatePromise of CustomValidatePromiseRule<'T>

    type ValidationError = { message: string }