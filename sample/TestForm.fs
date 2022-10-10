module TestForm

open Browser.Dom
open Feliz
open Fable.Core
open Fable.React
open Fable.FluentUI
open Fable.ReactHookForm.Form
open Fable.ReactHookForm.Controller
open Fable.ReactHookForm.Validation
open System.Text.RegularExpressions

type IDataNested = { Age: int }
type IData =
    { FirstName: string
      LastName: string
      Nested: IDataNested }

let defaultValues =
    { FirstName = "Steve"
      LastName = "Smith"
      Nested = { Age = 34 } }

[<ReactComponent>]
let TestForm () =
    let form =
        useForm [
            Mode OnChange
            DefaultValues defaultValues
        ]

    let validateSync v =
        if v = "James" then
            "Can't be James"
        else
            ""

    let validateAsync v =
        async {
            if v = "Jones" then
                return "Can't be Jones"
            else
                return ""
        }
        |> Async.StartAsPromise

    let firstName =
        useController form.control (fun x -> x.FirstName)
            [ Rules [
                  Required "First name is required"
                  MinLength { value = 4; message = "Min length 4" }
                  MaxLength
                      { value = 50
                        message = "Max length 50" }
                  Pattern
                      { value = new Regex("^[A-Z].?")
                        message = "Must start with capital" }
                  Validate validateSync
              ] ]

    let lastName =
        useController form.control (fun x -> x.LastName)
            [ Rules [ ValidateAsync validateAsync ] ]

    let age =
        useController form.control (fun x -> x.Nested.Age ) []

    let submit (v: IData) = console.log ("Submit", v)

    let error (v: obj) = console.log ("Error", v)

    let handleSubmit = form.handleSubmit submit error

    Html.div [
        Html.span "Data Entry Form"
        Html.input [
            prop.value firstName.field.value
            prop.onChange firstName.field.onChange
        ]
        TextField.textField [ TextField.Label "First Name"
                              TextField.Value firstName.field.value
                              TextField.OnChange firstName.field.onChangeEvent
                              TextField.ErrorMessage firstName.fieldState.error.message ] []
        TextField.textField [ TextField.Label "Last Name"
                              TextField.Value lastName.field.value
                              TextField.OnChange lastName.field.onChangeEvent
                              TextField.ErrorMessage lastName.fieldState.error.message ] []
        SpinButton.spinButton [ SpinButton.Label "Age"
                                SpinButton.Value (age.field.value.ToString()) ] []
        Button.defaultButton [ Button.OnClick(fun e -> form.reset ())
                               Button.Disabled(not form.formState.isDirty) ] [
            str "Reset"
        ]
        Button.primaryButton [ Button.OnClick handleSubmit
                               Button.Disabled(not form.formState.isDirty) ] [
            str "Submit"
        ]
    ]