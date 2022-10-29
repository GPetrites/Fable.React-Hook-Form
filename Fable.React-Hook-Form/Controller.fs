namespace Fable.ReactHookForm

open Fable.Core
open Fable.Core.JsInterop
open System.Text.RegularExpressions
open Fable.Core.JS

[<AutoOpen>]
module Controller =
    type UseControllerProps<'T, 'F> =
        | Control of Form.Control<'T>
        | Name of string
        | Rules of Rule<'F> list

    type private ControllerRenderPropsInternal<'F> =
        { name: string
          value: 'F
          onChange: (Browser.Types.Event -> 'F -> unit) }

    type ControllerRenderProps<'F> =
        { name: string
          value: 'F
          onChange: ('F -> unit)
          onChangeEvent: (Browser.Types.Event -> 'F -> unit) }

    type ControllerValidationError = { message: string }

    type private ControllerFieldStateInternal =
        { invalid: bool
          isTouched: bool
          isDirty: bool
          error: ControllerValidationError option }

    type ControllerFieldState =
        { invalid: bool
          isTouched: bool
          isDirty: bool
          error: ControllerValidationError }

    type private UseControllerReturnInternal<'F> =
        { field: ControllerRenderPropsInternal<'F>
          fieldState: ControllerFieldStateInternal }

    type UseControllerReturn<'T, 'F> =
        { field: ControllerRenderProps<'F>
          fieldState: ControllerFieldState
          name: string
          value: 'F
          onChange: ('F -> unit)
          onChangeEvent: (Browser.Types.Event -> 'F -> unit)
          invalid: bool
          isTouched: bool
          isDirty: bool
          error: ControllerValidationError
          errorMessage: string }

    let private mapResultToOption (result: Result<'T, string>) : string option =
        match result with
        | Ok _ -> None
        | Error msg -> Some msg

    let private mapAsyncResultToOption (result: Async<Result<'T, string>>) : Promise<string option> =
        async {
            let! result' = result

            return (mapResultToOption result')
        }
        |> Async.StartAsPromise

    let private mapRules =
        function
        | Validate f -> ValidateOption(f >> mapResultToOption)
        | ValidateAsync f -> ValidatePromise(f >> mapAsyncResultToOption)
        | MinLength (v, m) -> MinLength' { value = v; message = m }
        | MaxLength (v, m) -> MaxLength' { value = v; message = m }
        | Min (v, m) -> Min' { value = v; message = m }
        | Max (v, m) -> Max' { value = v; message = m }
        | Pattern (v, m) -> Pattern' { value = new Regex(v); message = m }
        | r -> r

    let private flattenRules prop =
        match prop with
        | Rules rules ->
            let newRules = rules |> List.map mapRules

            Rules !!(keyValueList CaseRules.LowerFirst newRules)
        | p -> p

    let internalUseController<'T, 'F> (props: UseControllerProps<'T, 'F> list) : UseControllerReturn<'T, 'F> =
        let newProps = props |> List.map flattenRules

        let r: UseControllerReturnInternal<'F> =
            import "useController" "react-hook-form" (keyValueList CaseRules.LowerFirst newProps)

        let error =
            r.fieldState.error
            |> Option.defaultValue { message = "" }

        { fieldState =
            { invalid = r.fieldState.invalid
              isTouched = r.fieldState.isTouched
              isDirty = r.fieldState.isDirty
              error = error }
          field =
            { name = r.field.name
              value = r.field.value
              onChange = !!r.field.onChange
              onChangeEvent = r.field.onChange }
          invalid = r.fieldState.invalid
          isTouched = r.fieldState.isTouched
          isDirty = r.fieldState.isDirty
          error = error
          errorMessage = error.message
          name = r.field.name
          value = r.field.value
          onChange = !!r.field.onChange
          onChangeEvent = r.field.onChange }

    let inline useController<'T, 'F>
        (control: Form.Control<'T>)
        (field: ('T -> 'F))
        (props: UseControllerProps<'T, 'F> list)
        : UseControllerReturn<'T, 'F> =
        let name =
            Experimental.namesofLambda (field)
            |> String.concat "."

        internalUseController<'T, 'F> (Control control :: Name name :: props)