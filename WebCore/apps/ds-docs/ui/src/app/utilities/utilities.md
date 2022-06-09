## Width

### Percentage-based
Set a percentage-based width from 1 to 100.

<div class="button-bar">
    <button class="btn btn-icon" ng-click="toggleWidthPercentage = !toggleWidthPercentage">
        <i class="material-icons">code</i>
        View Code
    </button>
</div>
<div class="example-block" ng-class="{'open': toggleWidthPercentage}">
    <div class="ui mb-4">
        <div class="form-group">
            <label>w-97 | This will take 97% of it's parent's width</label>
            <input class="form-control w-97" />
        </div>
    </div>

```html 
<div class="form-group">
    <label>w-97 | This will take 97% of it's parent's width</label>
    <input class="form-control w-97" />
</div>
```

</div>

### Pixel-based
<div class="button-bar">
    <button class="btn btn-icon" ng-click="toggleWidthPixel = !toggleWidthPixel">
        <i class="material-icons">code</i>
        View Code
    </button>
</div>
<div class="example-block" ng-class="{'open': toggleWidthPixel}">
    <div class="ui mb-4">
        <div class="form-group">
            <label>w-100px | This will take exactly 100px</label>
            <input class="form-control w-100px" />
        </div>
    </div>

```html 
 <div class="form-group">
    <label>w-100px | This will take exactly 100px</label>
    <input class="form-control w-100px" />
</div>
```

</div>

#### Other pixel width options
.w-80px

## Common Patterns
### Data Item
<div class="button-bar">
    <button class="btn btn-icon" ng-click="toggleDataItem = !toggleDataItem">
        <i class="material-icons">code</i>
        View Code
    </button>
</div>
<div class="example-block" ng-class="{'open': toggleDataItem}">
    <div class="ui mb-4">
        <div class="card-data bordered">
            <div class="item">
                10/26/2018
            </div>
            <div class="item">
                12:00 PM
            </div>
        </div>
    </div>

```html 
<div class="card-data bordered">
            <div class="item">
                10/26/2018
            </div>
            <div class="item">
                12:00 PM
            </div>
        </div>
```

</div>


### Muted Label Content
<div class="button-bar">
    <button class="btn btn-icon" ng-click="toggleMutedLabel = !toggleMutedLabel">
        <i class="material-icons">code</i>
        View Code
    </button>
</div>
<div class="example-block" ng-class="{'open': toggleMutedLabel}">
    <div class="ui mb-4">
        <div class="card-data">
            <div class="item">
                <label>DATE:</label> 10/26/2018
            </div>
            <div class="item">
                <label>TIME:</label> 12:00 PM
            </div>
        </div>
        <br/>
        <div class="card-data bordered">
            <div class="item">
                <label>DATE:</label> 10/26/2018
            </div>
            <div class="item">
                <label>TIME:</label> 12:00 PM
            </div>
        </div>
        <br/>
        <div class="card-data block">
            <div class="item">
                <label>DATE:</label> 10/26/2018
            </div>
            <div class="item">
                <label>TIME:</label> 12:00 PM
            </div>
        </div>
    </div>

```html 
<div class="card-data">
    <div class="item">
        <label>DATE:</label> 10/26/2018
    </div>
    <div class="item">
        <label>TIME:</label> 12:00 PM
    </div>
</div>
<br/>
<div class="card-data bordered">
    <div class="item">
        <label>DATE:</label> 10/26/2018
    </div>
    <div class="item">
        <label>TIME:</label> 12:00 PM
    </div>
</div>
<div class="card-data block">
    <div class="item">
        <label>DATE:</label> 10/26/2018
    </div>
    <div class="item">
        <label>TIME:</label> 12:00 PM
    </div>
</div>
```

</div>

| Class             | Description                                                                                |
| ------------------|--------------------------------------------------------------------------------------------|
|`.card-data`       |Aligns content side by side                                                                 |
|`.bordered`        |Adds a light gray seperator between `.item` children when used with `.card-data`            |
|`.centered`        |Aligns content to the center                                                                |
|`.item`            |Content seperator, Use within `.card-data`                                                  |
|`label`            |spaces label from content, adds text-muted, use within `.card-data`                         |
|`.block`           |Shows `.item` on individual lines                                                           |



### Time
_ToDo: Clean up alignment with required fields
<div class="button-bar">
    <button class="btn btn-icon" ng-click="toggleTime = !toggleTime">
        <i class="material-icons">code</i>
        View Code
    </button>
</div>
<div class="example-block" ng-class="{'open': toggleTime}">
    <div class="ui mb-4">
       <div class="row inline-form-group">
            <div class="col-sm">
                <div class="form-group">
                    <label class="form-control-label">Start Time</label>
                    <input type="text" class="form-control" 
                        dsMomentTimeInput 
                        name="timeStart"
                        required
                        dsFormControlValidator />
                </div>
            </div>
            <div class="col-sm-auto inline-form-text">to</div>
            <div class="col-sm">
                <div class="form-group">
                    <label class="form-control-label">End Time</label>
                    <input type="text" class="form-control" 
                        dsMomentTimeInput 
                        name="timeEnd"
                        required
                        dsFormControlValidator />
                </div>
            </div>
       </div>
    </div>

```html 
<div class="row">
    <div class="col-sm">
        <div class="form-group">
            <label class="form-control-label">Start Time</label>
            <input type="text" class="form-control" 
                dsMomentTimeInput 
                name="timeStart"
                required
                dsFormControlValidator />
        </div>
    </div>
    <div class="col-sm-auto px-0 align-self-center">to</div>
    <div class="col-sm">
        <div class="form-group">
            <label class="form-control-label">End Time</label>
            <input type="text" class="form-control" 
                dsMomentTimeInput 
                name="timeEnd"
                required
                dsFormControlValidator />
        </div>
    </div>
</div>
```

</div>

Lunch/Break
Start TIme, Stop Time 

