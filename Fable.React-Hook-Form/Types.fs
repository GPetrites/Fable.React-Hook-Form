namespace Fable.ReactHookForm.Types

open System.Text.RegularExpressions
open Fable.Core.JS

type IntRule = { value: int; message: string }
type DecimalRule = { value: decimal; message: string }
type PatternRule = { value: Regex; message: string }
type CustomValidateRule = string -> string option
type CustomValidateAsyncRule = string -> Async<string option>
type CustomValidatePromiseRule = string -> Promise<string option>
