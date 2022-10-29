namespace Fable.ReactHookForm

open Fable.Core
open Fable.Core.JsInterop
open System
open Fable.Core.JS

[<AutoOpen>]
module Form =

    [<StringEnum>]
    type FormValidation =
        | OnBlur
        | OnChange
        | OnSubmit
        | OnTouched
        | All

    type UseFormProps<'T> =
        | Mode of FormValidation
        | DefaultValues of 'T

    type FormResetProps<'T> =
        | Values of 'T
        | KeepErrors of bool
        | KeepDirty of bool
        | KeepDirtyValues of bool
        | KeepValues of bool
        | KeepDefaultValues of bool
        | KeepIsSubmitted of bool
        | KeepTouched of bool
        | KeepIsValid of bool
        | KeepSubmitCount of bool

    type FormState<'T> =
        { isDirty: bool
          isSubmitted: bool
          isSubmitSuccessful: bool
          submitCount: int
          isSubmitting: bool
          isValidating: bool
          isValid: bool }

    type Control<'T> = { register: obj; unregister: obj }

    type SubmitHandler<'T> = 'T -> unit
    type SubmitErrorHandler = obj -> unit
    type private SubmitHandlerPromise<'T> = 'T -> Promise<unit>
    type private SubmitErrorHandlerPromise = obj -> Promise<unit>
    type SubmitHandlerAsync<'T> = 'T -> Async<unit>
    type SubmitErrorHandlerAsync = obj -> Async<unit>

    type private UseFormReturnInternal<'T> =
        { control: Control<'T>
          handleSubmit: SubmitHandlerPromise<'T> -> SubmitErrorHandlerPromise -> Func<Browser.Types.Event, unit>
          reset: obj -> obj -> unit
          formState: FormState<'T>
          getValues: obj }

    type UseFormReturn<'T> =
        { control: Control<'T>
          handleSubmit: SubmitHandler<'T> -> SubmitErrorHandler -> Browser.Types.Event -> unit
          handleSubmitAsync: SubmitHandlerAsync<'T> -> SubmitErrorHandlerAsync -> Browser.Types.Event -> unit
          reset: FormResetProps<'T> list -> unit
          formState: FormState<'T>
          getValues: obj }


    let useForm<'T> (props: UseFormProps<'T> list) : UseFormReturn<'T> =
        let r: UseFormReturnInternal<'T> =
            import "useForm" "react-hook-form" (keyValueList CaseRules.LowerFirst props)

        let handleSubmit (s: SubmitHandler<'T>) (e: SubmitErrorHandler) = //
            (r.handleSubmit !!s !!e).Invoke

        let handleSubmitAsync s e =
            let sp = s >> Async.StartAsPromise
            let ep = e >> Async.StartAsPromise
            (r.handleSubmit sp ep).Invoke

        let reset (opts: FormResetProps<'T> list) =
            let (v, o) =
                opts
                |> List.partition (function
                    | Values _ -> true
                    | _ -> false)

            let newValue =
                List.tryHead v
                |> Option.map (function
                    | Values v -> Some v
                    | _ -> None)
                |> Option.flatten


            keyValueList CaseRules.LowerFirst o
            |> r.reset newValue

        { control = r.control
          handleSubmit = handleSubmit
          handleSubmitAsync = handleSubmitAsync
          reset = reset
          formState = r.formState
          getValues = r.getValues }