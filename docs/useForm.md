# useForm

```useForm``` provides Fable bindings for [```useForm```](https://react-hook-form.com/api/useform)

## [```useForm props```](https://react-hook-form.com/api/useform)

**props :** ```UseFormProps<'T> list```
<br>Options for defining the form and state management.

**Returns** ```UseFormReturn<'T>```
<br>An object which provides state management for an entire input form. The object will track conditions such as:
- Has the user made changes to the form data (```isDirty```)
- Does the form pass all validations (```isValid```)

The state management object also provides methods for interacting with the form state including:
- Submitting the form (```handleSubmit```)
- Resetting the form states (```reset```)

To define a form, first define a type and a default value for the form. Next, call useForm passing in the update mode and default object.

```fsharp
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
let MyComponent () =
    let form =
        useForm [
            Mode OnChange
            DefaultValues defaultValues
        ]

```

## [```form.formState```](https://react-hook-form.com/api/useform/formstate)

Used to examine the current condition of the form. Can be used to toggle buttons to indicate whether a form is ready to be submitted

**Example**

```fsharp
Button.primaryButton
    [ Button.OnClick handleSubmit
      Button.Disabled(not form.formState.isDirty) ]
    [ str "Submit" ]
```
## [```form.handleSubmit submitHandler errorHandler```](https://react-hook-form.com/api/useform/reset)

Creates a Function which can be usee to initiate submitting a form.

**submitHandler :** ```'T -> unit```
<br>Function to handle submit when the form is in a valid state

> ```'T``` is the type supplied as ```DefaultValues```

**errorHandler :** ```obj -> unit```
<br>Function to handle submit when the for is in an invalid state

**Returns :** ```Browser.Types.Event -> unit```
<br>A function to call to trigger a submit

**Example**

```fsharp
let submit (v: IData) = 
    ...
    ()

let error (v: obj) = 
    ...
    ()

let handleSubmit = form.handleSubmit submit error

Html.div [
    ...
    Button.primaryButton
        [ Button.OnClick handleSubmit ]
        [ str "Submit" ]
    ...
]
```

## [```form.handleSubmitAsync submitHandler errorHandler```](https://react-hook-form.com/api/useform/reset)

Creates a Function which can be usee to initiate submitting a form.

**submitHandler :** ```'T -> Async<unit>```
<br>Function to handle submit when the form is in a valid state

> ```'T``` is the type supplied as ```DefaultValues```

**errorHandler :** ```obj -> Async<unit>```
<br>Function to handle submit when the for is in an invalid state

**Returns :** ```Browser.Types.Event -> Async<unit>```
<br>A function to call to trigger a submit

**Example**

```fsharp
let submit (v: IData) = 
    async {
        ...
    }

let error (v: obj) = 
    async {
        ...
    }

let handleSubmit = form.handleSubmitAsync submit error

Html.div [
    ...
    Button.primaryButton
        [ Button.OnClick handleSubmit ]
        [ str "Submit" ]
    ...
]
```

## [```form.reset opts```](https://react-hook-form.com/api/useform/reset)

Function that will return the form to it's initial state

**opts :** ```ResetProps<'T> -> unit```
<br>Options to pass to the reset function

**Example**

```fsharp
Button.defaultButton
    [ Button.OnClick(fun e -> form.reset []) ]
    [ str "Reset" ]
```