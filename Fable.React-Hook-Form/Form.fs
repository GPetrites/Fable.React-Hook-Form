namespace Fable.ReactHookForm

open Fable.Core
open Fable.Core.JsInterop
open System

module Form =

    [<StringEnum>]
    type ValidationMode =
        | OnBlur
        | OnChange
        | OnSubmit
        | OnTouched
        | All

    type UseFormProps<'T> =
        | Mode of ValidationMode
        | DefaultValues of 'T

    type FormState<'T> =
        { isDirty: bool
          isSubmitted: bool
          isSubmitSuccessful: bool
          submitCount: int
          isSubmitting: bool
          isValidating: bool
          isValid: bool }

    type Control = { register: obj; unregister: obj }

    type SubmitHandler<'T> = 'T -> unit
    type SubmitErrorHandler = obj -> unit

    type private UseFormReturnInternal<'T> =
        { control: Control
          handleSubmit: SubmitHandler<'T> -> SubmitErrorHandler -> Func<Browser.Types.Event, unit>
          reset: unit -> unit
          formState: FormState<'T>
          getValues: obj }

    type UseFormReturn<'T> =
        { control: Control
          handleSubmit: SubmitHandler<'T> -> SubmitErrorHandler -> Browser.Types.Event -> unit
          reset: unit -> unit
          formState: FormState<'T>
          getValues: obj }

    let useForm<'T> (props: UseFormProps<'T> list) : UseFormReturn<'T> =
        let r: UseFormReturnInternal<'T> =
            import "useForm" "react-hook-form" (keyValueList CaseRules.LowerFirst props)

        { control = r.control
          handleSubmit = (fun s e -> (r.handleSubmit s e).Invoke)
          reset = r.reset
          formState = r.formState
          getValues = r.getValues }