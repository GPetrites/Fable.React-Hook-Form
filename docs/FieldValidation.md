# Field Validation

Field validation rules are defined when registering a field using ```useController``` by passing in an array of validation rules.

Ref: https://react-hook-form.com/api/useform/register#options

**Example :**

```fsharp
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
```

## Custom validation

Custom validation can be perform either synchronous or asynchronous.

### Validate validate

**validate :** 'F -> string option
<br>A function which receives the current value of the field and returns either None, if no error, or Some which contains the error message

**Example**

```fsharp
let validate v =
    if ...
    then Some "Not valid"
    else None

let fld = useController form.control (fun f -> f.Field) 
    [ Rules
        [ Validate validate ]
    ]
```

### ValidateAsync validateAsync

**validate :** 'F -> Async<string option>
<br>A function which receives the current value of the field and returns either None, if no error, or Some which contains the error message

**Example**

```fsharp
let validateAsync v =
    async {
        let result =
            if ...
            then Some "Not valid"
            else None

        return result
    }

let fld = useController form.control (fun f -> f.Field) 
    [ Rules
        [ ValidateAsync validateAsync ]
    ]
```