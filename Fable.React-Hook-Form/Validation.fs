namespace Fable.ReactHookForm

open System.Text.RegularExpressions
open Fable.Core.JS

[<AutoOpen>]
module Validation =
    type RuleIntProp = { value: int; message: string }
    type RuleDecimalProp = { value: decimal; message: string }
    type RulePatternProp = { value: Regex; message: string }
    type RuleValidateResult<'T> = 'T -> Result<'T, string>
    type RuleValidateOption<'T> = 'T -> Option<string>
    type RuleValidateAsyncResult<'T> = 'T -> Async<Result<'T, string>>
    type RuleValidatePromiseOption<'T> = 'T -> Promise<string option>

    type Rule<'T> =
        | Required of string

        | MaxLength of int * string
        | [<CompiledName("maxLength")>] MaxLength' of RuleIntProp

        | MinLength of int * string
        | [<CompiledName("maxLength")>] MinLength' of RuleIntProp

        | Max of decimal * string
        | [<CompiledName("max")>] Max' of RuleDecimalProp

        | Min of decimal * string
        | [<CompiledName("mmin")>] Min' of RuleDecimalProp

        | Pattern of string * string
        | [<CompiledName("pattern")>] Pattern' of RulePatternProp

        | Validate of RuleValidateResult<'T>
        | [<CompiledName("validate")>] ValidateOption of RuleValidateOption<'T>
        | [<CompiledName("validate")>] ValidateAsync of RuleValidateAsyncResult<'T>
        | [<CompiledName("validate")>] ValidatePromise of RuleValidatePromiseOption<'T>
