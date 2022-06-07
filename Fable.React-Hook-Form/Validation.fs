namespace Fable.ReactHookForm

open System.Text.RegularExpressions
open Fable.Core.JS

module Validation =
    type IntRule = { value: int; message: string }
    type DecimalRule = { value: decimal; message: string }
    type PatternRule = { value: Regex; message: string }
    type CustomValidateRule = string -> string
    type CustomValidateAsyncRule = string -> Promise<string>

    type Rule =
        | Required of string
        | MaxLength of IntRule
        | MinLength of IntRule
        | Max of DecimalRule
        | Min of DecimalRule
        | Pattern of PatternRule
        | Validate of CustomValidateRule
        | [<CompiledName("validate")>] ValidateAsync of CustomValidateAsyncRule

    type ValidationError = { message: string }