namespace Fable.ReactHookForm

open Fable.Core
open Fable.Core.JsInterop
open Fable.ReactHookForm.Validation

module Controller =
    type UseControllerProps<'F> =
        | Control of Form.Control
        | Name of string
        | Rules of Rule list

    type private ControllerRenderPropsInternal<'F> =
        { name: string
          value: 'F
          onChange: (Browser.Types.Event -> 'F -> unit) }

    type ControllerRenderProps<'F> =
        { name: string
          value: 'F
          onChange: ('F -> unit)
          onChangeEvent: (Browser.Types.Event -> 'F -> unit) }

    type ControllerFieldStateInternal =
        { invalid: bool
          isTouched: bool
          isDirty: bool
          error: ValidationError option }

    type ControllerFieldState =
        { invalid: bool
          isTouched: bool
          isDirty: bool
          error: ValidationError }

    type private UseControllerReturnInternal<'F> =
        { field: ControllerRenderPropsInternal<'F>
          fieldState: ControllerFieldStateInternal }

    type UseControllerReturn<'F> =
        { field: ControllerRenderProps<'F>
          fieldState: ControllerFieldState }

    let private flattenRules prop =
        match prop with
        | Rules rules -> Rules !!(keyValueList CaseRules.LowerFirst rules)
        | p -> p

    let private realizeError e : ValidationError =
        match e with
        | None -> { message = "" }
        | Some e -> e

    let useController<'F> (props: UseControllerProps<'F> list) : UseControllerReturn<'F> =
        let newProps = props |> List.map flattenRules

        let r: UseControllerReturnInternal<'F> =
            import "useController" "react-hook-form" (keyValueList CaseRules.LowerFirst newProps)

        { fieldState =
            { invalid = r.fieldState.invalid
              isTouched = r.fieldState.isTouched
              isDirty = r.fieldState.isDirty
              error = (realizeError r.fieldState.error) }
          field =
            { name = r.field.name
              value = r.field.value
              onChange = !!r.field.onChange
              onChangeEvent = r.field.onChange } }