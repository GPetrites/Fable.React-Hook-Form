# useForm

```useForm``` provides Fable bindings for [```useForm```](https://react-hook-form.com/api/useform)

```useForm``` create an object to provide state management for an entire input form. The object will track things like:
- Has the user made changes to the form data (```isDirty```)
- Does the form pass all validations (```isValid```)

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

## Return value from ```useForm```

<table>
<tr>
    <th>Function or Value</th>
    <th>Description</th>
</tr>
<tr>
<td valign="top">

[```form.formState```](https://react-hook-form.com/api/useform/formstate)

</td>
<td>

Used to examine the current condition of the form. Can be used to toggle buttons to indicate whether a form is ready to be submitted

**Example**

```fsharp
Button.primaryButton
    [ Button.OnClick handleSubmit
      Button.Disabled(not form.formState.isDirty) ]
    [ str "Submit" ]
```

</td>
</tr>
<tr>
<td valign="top">

[```form.reset opts```](https://react-hook-form.com/api/useform/reset)

</td>
<td>

Function that will return the form to it's initial state

**opts :** ```ResetProps<'T> -> unit```
<br>Options to pass to the reset function

**Example**

```fsharp
Button.defaultButton
    [ Button.OnClick(fun e -> form.reset []) ]
    [ str "Reset" ]
```

</td>
</tr>
<tr>
<td valign="top">

[```form.handleSubmit submitHandler errorHandler```](https://react-hook-form.com/api/useform/reset)

</td>
<td>

Function to use to create a 

**submitHandler :** ```'T -> unit```
<br>Function to handle submit when the form is in a valid state

> ```'T``` is the type supplied as ```DefaultValues```

**errorHandler :** ```obj -> unit```
<br>Function to handle submit when the for is in an invalid state

**Returns :** ```Browser.Types.Event -> unit```
<br>A function to call to trigger a submit

**Example**

```fsharp
let submit (v: IData) = console.log ("Submit", v)
let error (v: obj) = console.log ("Error", v)

let handleSubmit = form.handleSubmit submit error

Html.div [
    ...
    Button.primaryButton
        [ Button.OnClick handleSubmit ]
        [ str "Submit" ]
    ...
]
```

</td>
</tr>
</table>

