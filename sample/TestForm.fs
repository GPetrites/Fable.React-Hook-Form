module TestForm

open Browser.Dom
open Feliz
open Fable.Core
open Fable.React
open Fable.FluentUI
open Fable.ReactHookForm
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
            Error "Can't be James"
        else
            Ok v

    let validateAsync v =
        async {
            if v = "Jones" then
                return Error "Can't be Jones"
            else
                return Ok v
        }

    let firstName =
        useController
            form.control
            (fun x -> x.FirstName)
            [ Rules [
                  Required "First name is required"
                  MinLength(4, "Min length 4")
                  MaxLength(50, "Max length 50")
                  Pattern("^[A-Z].?", "Must start with capital")
                  Validate validateSync
              ] ]

    let lastName =
        useController form.control (fun x -> x.LastName) [ Rules [ ValidateAsync validateAsync ] ]

    let age = useController form.control (fun x -> x.Nested.Age) []

    let submit (v: IData) =
        console.log ("Submit", v)
        form.reset [ Values v ]

    let error (v: obj) = console.log ("Error", v)

    let handleSubmit = form.handleSubmit submit error

    Html.div [
        Html.span "Data Entry Form"
        Html.input [
            prop.value firstName.value
            prop.onChange firstName.onChange
        ]
        TextField.textField [ TextField.Label "First Name"
                              TextField.Value firstName.value
                              TextField.OnChange firstName.onChangeEvent
                              TextField.ErrorMessage firstName.errorMessage ] []
        TextField.textField [ TextField.Label "Last Name"
                              TextField.Value lastName.value
                              TextField.OnChange lastName.onChangeEvent
                              TextField.ErrorMessage lastName.errorMessage ] []
        SpinButton.spinButton [ SpinButton.Label "Age"
                                SpinButton.Value(age.value.ToString()) ] []
        Button.defaultButton [ Button.OnClick(fun e -> form.reset [])
                               Button.Disabled(not form.formState.isDirty) ] [
            str "Reset"
        ]
        Button.primaryButton [ Button.OnClick handleSubmit
                               Button.Disabled(not form.formState.isDirty) ] [
            str "Submit"
        ]
    ]