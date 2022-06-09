# Adress Blocks
<p class="description">Every address block should have a consistent layout.</p>

## Layout

### Standard
Country is always in the first position because it determines if State or County are valid fields.
<div class="button-bar">
    <button class="btn btn-icon" ng-click="toggleStandardAddress = !toggleStandardAddress">
        <i class="material-icons">code</i>
        View Code
    </button>
</div>
<div class="example-block" ng-class="{'open': toggleStandardAddress}">
    <div class="ui mb-4">
        <div class="row">
            <div class="col-md-12">
                <div class="form-group">
                    <label class="form-control-label" for="country">Country</label>
                    <select type="text" class="form-control custom-select" id="country" >
                        <option value="0"></option>
                        <option value="1">Country options here</option>
                    </select>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="form-group">
                    <label class="form-control-label" for="addressLineOne">Address</label>
                    <input type="text" class="form-control" id="addressLineOne" />
                </div>
                <div class="form-group">
                    <input type="text" class="form-control" id="addressLineTwo" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label class="form-control-label" for="city">City</label>
                    <input type="text" class="form-control" id="city" />
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label class="form-control-label" for="state">State/Province</label>
                    <select type="text" class="form-control custom-select" id="state" >
                        <option value="0"></option>
                        <option value="1">State options here</option>
                    </select>
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label class="form-control-label" for="zip">Zip/Postal Code</label>
                    <input type="text" class="form-control" id="zip" />
                </div>
            </div>
        </div>
    </div>

```html
<div class="row">
    <div class="col-md-12">
        <div class="form-group">
            <label class="form-control-label" for="addressLineOne">Address</label>
            <input type="text" class="form-control" id="addressLineOne" />
        </div>
        <div class="form-group">
            <input type="text" class="form-control" id="addressLineTwo" />
        </div>
    </div>
</div>
<div class="row">
    <div class="col-md-4">
        <div class="form-group">
            <label class="form-control-label" for="city">City</label>
            <input type="text" class="form-control" id="city" />
        </div>
    </div>
     <div class="col-md-4">
        <div class="form-group">
            <label class="form-control-label" for="state">State/Province</label>
            <select type="text" class="form-control custom-select" id="state" >
                <option value="0"></option>
                <option value="1">State options here</option>
            </select>
        </div>
    </div>
    <div class="col-md-4">
        <div class="form-group">
            <label class="form-control-label" for="zip">Zip/Postal Code</label>
            <input type="text" class="form-control" id="zip" />
        </div>
    </div>
</div>
```
</div>

### County
Add County directly after Zip/Postal Code

<div class="button-bar">
    <button class="btn btn-icon" ng-click="toggleCountyAddress = !toggleCountyAddress">
        <i class="material-icons">code</i>
        View Code
    </button>
</div>
<div class="example-block" ng-class="{'open': toggleCountyAddress}">
    <div class="ui mb-4">
        <div class="row">
            <div class="col-md-12">
                <div class="form-group">
                    <label class="form-control-label" for="addressLineOne">Address</label>
                    <input type="text" class="form-control" id="addressLineOne" />
                </div>
                <div class="form-group">
                    <input type="text" class="form-control" id="addressLineTwo" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                <div class="form-group">
                    <label class="form-control-label" for="city">City</label>
                    <input type="text" class="form-control" id="city" />
                </div>
            </div>
            <div class="col-md-3">
                <div class="form-group">
                    <label class="form-control-label" for="state">State/Province</label>
                    <select type="text" class="form-control custom-select" id="state" >
                        <option value="0"></option>
                        <option value="1">State options here</option>
                    </select>
                </div>
            </div>
            <div class="col-md-3">
                <div class="form-group">
                    <label class="form-control-label" for="zip">Zip/Postal Code</label>
                    <input type="text" class="form-control" id="zip" />
                </div>
            </div>
            <div class="col-md-3">
                <div class="form-group">
                    <label class="form-control-label" for="county">County</label>
                    <select class="form-control custom-select" id="county" >
                        <option value="0"></option>
                        <option value="1">County options here</option>
                    </select>
                </div>
            </div>
        </div>
    </div>

```html
<div class="row">
    <div class="col-md-12">
        <div class="form-group">
            <label class="form-control-label" for="addressLineOne">Address</label>
            <input type="text" class="form-control" id="addressLineOne" />
        </div>
        <div class="form-group">
            <input type="text" class="form-control" id="addressLineTwo" />
        </div>
    </div>
</div>
<div class="row">
    <div class="col-md-4">
        <div class="form-group">
            <label class="form-control-label" for="city">City</label>
            <input type="text" class="form-control" id="city" />
        </div>
    </div>
    <div class="col-md-3">
        <div class="form-group">
            <label class="form-control-label" for="state">State/Province</label>
            <select type="text" class="form-control custom-select" id="state" >
                <option value="0"></option>
                <option value="1">State options here</option>
            </select>
        </div>
    </div>
    <div class="col-md-3">
        <div class="form-group">
            <label class="form-control-label" for="zip">Zip/Postal Code</label>
            <input type="text" class="form-control" id="zip" />
        </div>
    </div>
    <div class="col-md-4">
        <div class="form-group">
            <label class="form-control-label" for="county">County</label>
            <select class="form-control custom-select" id="county" >
                <option value="0"></option>
                <option value="1">County options here</option>
            </select>
        </div>
    </div>
</div>
```

</div>

## Validation
Street Address validation follows standard. Examples are sparing. Validation should be handled client-side if at all possible.
One (mostly) complete example is on https://zmonster/payroll/CompanyDivision.aspx > + Address

#### Country
Determines if State or county selection is valid.

#### State/Province
If Country is USA or Canada, this field is required.
Determines if Zip/Postal Code is a valid field.

#### Zip/Postal Code
If Country is USA or Canada, this field is required.
Valid formats are #####, #####-#### (USA) and XXX-XXX (Canada)
