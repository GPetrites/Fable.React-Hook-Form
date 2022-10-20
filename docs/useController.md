# useController

```useController``` provides Fable bindings for [```useController```](https://react-hook-form.com/api/usecontroller).

## [```useController form field props```](https://react-hook-form.com/api/usecontroller)

Returns an object which provides state management for a single values on the form.

**control :** ```Controller.Control<'T>```
<br>The form state management object

**field :** ```'T -> 'F```
<br>A function which references the field on the object to be managed by this controller.

> Reflection will be used to translate this function into the FilePath passed in to React-Hook-Form.

**props :** ```UseControllerProps<'T,'F> list```
<br>A list of additional properties for the controller, including ```Rules``` for validation

**returns :** ```UseControllerReturn<'T,'F>```
<br>The state management object for the field

> For ease of use, the binding flattens the controller, combining the nested ```field``` and ```fieldState``` properties into a single object.

**Example**

```fsharp
let firstName = useController form.controller (fun x -> x.FirstName) []

Html.div [
    ...
    TextField.textField 
        [ TextField.Label "First Name"
          TextField.Value firstName.value
          TextField.OnChange firstName.onChangeEvent
          TextField.ErrorMessage firstName.error.message ] []
    ...
]
```

## controller.onChanged newValue

Sets the field to a new value

> Use this function when you are passing in just a value

**newValue :** ```'F```
<br>The new value

**Example**

```fsharp
    Html.div [
        ...
        Html.input [
            prop.value firstName.value
            prop.onChange firstName.onChange
        ]
        ...
    ]
```

## controller.onChangedEvent event newValue

Sets the field to a new value

> Use this function when you need to pass in an event and a value

**event :** ```Browser.Types.Event```
<br>Event triggering the update

**newValue :** ```'F```
<br>The new value

**Example**

```fsharp
    Html.div [
        ...
        TextField.textField [ 
            TextField.Value firstName.value
            TextField.OnChange firstName.onChangeEvent ] []
        ...
    ]
```

## controller.errorMessage
If the field value is in error, the message for that error

**Example**

```fsharp
    Html.div [
        ...
        TextField.textField [ 
            TextField.Value firstName.value
            TextField.OnChange firstName.onChangeEvent
            TextField.ErrorMessage firstName.errorMessage ] []

        ...
    ]
```


## controller.value

The current value for the field

**Example**

```fsharp
    Html.div [
        ...
        TextField.textField [ 
            TextField.Value firstName.value
            TextField.OnChange firstName.onChangeEvent ] []
        ...
    ]
```
