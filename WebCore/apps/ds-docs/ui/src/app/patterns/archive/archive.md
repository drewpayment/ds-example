# Archive

## Archiving Workflows
A workflow is a task a user needs to complete. Once completed, the data is kept as a record. 
<div class="button-bar">
    <button class="btn btn-icon" ng-click="toggleArchive = !toggleArchive">
        <i class="material-icons">code</i>
        View Code
    </button>
</div>
<div class="example-block" ng-class="{'open': toggleArchive}">
    <div class="ui mb-4">
        <div class="overflow-list">
            <div class="ds-card">
                <div class="ds-card-header header-bar border-top-primary">
                    <div class="ds-card-header-row">
                        <div class="ds-card-title">
                            <h1>Card Header</h1>
                        </div>
                        <div class="ds-card-action">
                            <button class="btn btn-outline-primary" ng-click="archive = !archive">
                                <span ng-if="!archive">View History</span>
                                <span ng-if="archive">View Active</span>
                            </button>
                        </div>
                    </div>
                </div>
                <div class="ds-card-body">
                    <div class="ds-card-content">
                        <div class="overflow-list">
                            <div class="row" ng-if="!archive">
                                <div class="col-sm-6 col-md-3">
                                    <div class="ds-card ds-card-widget ds-object card-height-fix-sm clickable actionable info">
                                        <div class="ds-card-container"> 
                                            <div class="ds-card-header d-block align-y-top">
                                                <div class="ds-card-subtitle">
                                                    <div  class="split-content">
                                                        <div class="top">
                                                            <div class="object-card-avatar">
                                                                <mat-icon class="avatar mat-icon material-icons"  role="img" aria-hidden="true">description</mat-icon>
                                                            </div>
                                                            <div class="font-lg text-center mb-4">Workflow</div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6 col-md-3">
                                    <div class="ds-card ds-card-widget ds-object card-height-fix-sm clickable actionable info">
                                        <div class="ds-card-container"> 
                                            <div class="ds-card-header d-block align-y-top">
                                                <div class="ds-card-subtitle">
                                                    <div  class="split-content">
                                                        <div class="top">
                                                            <div class="object-card-avatar">
                                                                <mat-icon class="avatar mat-icon material-icons"  role="img" aria-hidden="true">description</mat-icon>
                                                            </div>
                                                            <div class="font-lg text-center mb-4">Workflow 2</div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row" ng-if="archive">
                                <div class="col-sm-6 col-md-3">
                                    <div class="ds-card ds-card-widget ds-object card-height-fix-sm clickable actionable info">
                                        <div class="ds-card-container"> 
                                            <div class="ds-card-header d-block align-y-top">
                                                <div class="ds-card-subtitle">
                                                    <div  class="split-content">
                                                        <div class="top">
                                                            <div class="object-card-avatar">
                                                                <mat-icon class="avatar mat-icon material-icons"  role="img" aria-hidden="true">description</mat-icon>
                                                            </div>
                                                            <div class="font-lg text-center mb-4">Archived Workflow</div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

```html
<!-- Angular -->
<div class="overflow-list">
    <ds-card border="top">
        <ds-card-header>
            <ds-card-header-title>Card Header</ds-card-header-title>
            <ds-card-title-action>
                <button class="btn btn-outline-primary" ng-click="archive = !archive">
                    <span ng-if="!archive">View History</span>
                    <span ng-if="archive">View Active</span>
                </button>
            </ds-card-title-action>
        </ds-card-header>
        <ds-card-content>
            <div class="overflow-list">
                <div class="row" ng-if="!archive">
                    <div class="col-sm-6 col-md-3">
                        <div class="ds-card ds-card-widget ds-object card-height-fix-sm clickable actionable info">
                            <div class="ds-card-container"> 
                                <div class="ds-card-header d-block align-y-top">
                                    <div class="ds-card-subtitle">
                                        <div  class="split-content">
                                            <div class="top">
                                                <div class="object-card-avatar">
                                                    <mat-icon class="avatar mat-icon material-icons"  role="img" aria-hidden="true">description</mat-icon>
                                                </div>
                                                <div class="font-lg text-center mb-4">Workflow</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-md-3">
                        <div class="ds-card ds-card-widget ds-object card-height-fix-sm clickable actionable info">
                            <div class="ds-card-container"> 
                                <div class="ds-card-header d-block align-y-top">
                                    <div class="ds-card-subtitle">
                                        <div  class="split-content">
                                            <div class="top">
                                                <div class="object-card-avatar">
                                                    <mat-icon class="avatar mat-icon material-icons"  role="img" aria-hidden="true">description</mat-icon>
                                                </div>
                                                <div class="font-lg text-center mb-4">Workflow 2</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" ng-if="archive">
                    <div class="col-sm-6 col-md-3">
                        <div class="ds-card ds-card-widget ds-object card-height-fix-sm clickable actionable info">
                            <div class="ds-card-container"> 
                                <div class="ds-card-header d-block align-y-top">
                                    <div class="ds-card-subtitle">
                                        <div  class="split-content">
                                            <div class="top">
                                                <div class="object-card-avatar">
                                                    <mat-icon class="avatar mat-icon material-icons"  role="img" aria-hidden="true">description</mat-icon>
                                                </div>
                                                <div class="font-lg text-center mb-4">Archived Workflow</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ds-card-content>
        </ds-card>
    </div>
</div>

<!-- Legacy -->
<div class="overflow-list">
    <div class="ds-card">
        <div class="ds-card-header header-bar border-top-primary">
            <div class="ds-card-header-row">
                <div class="ds-card-title">
                    <h1>Card Header</h1>
                </div>
                <div class="ds-card-action">
                    <button class="btn btn-outline-primary" ng-click="archive = !archive">
                        <span ng-if="!archive">View History</span>
                        <span ng-if="archive">View Active</span>
                    </button>
                </div>
            </div>
        </div>
        <div class="ds-card-body">
            <div class="ds-card-content">
                <div class="overflow-list">
                    <div class="row" ng-if="!archive">
                        <div class="col-sm-6 col-md-3">
                            <div class="ds-card ds-card-widget ds-object card-height-fix-sm clickable actionable info">
                                <div class="ds-card-container"> 
                                    <div class="ds-card-header d-block align-y-top">
                                        <div class="ds-card-subtitle">
                                            <div  class="split-content">
                                                <div class="top">
                                                    <div class="object-card-avatar">
                                                        <mat-icon class="avatar mat-icon material-icons"  role="img" aria-hidden="true">description</mat-icon>
                                                    </div>
                                                    <div class="font-lg text-center mb-4">Workflow</div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-3">
                            <div class="ds-card ds-card-widget ds-object card-height-fix-sm clickable actionable info">
                                <div class="ds-card-container"> 
                                    <div class="ds-card-header d-block align-y-top">
                                        <div class="ds-card-subtitle">
                                            <div  class="split-content">
                                                <div class="top">
                                                    <div class="object-card-avatar">
                                                        <mat-icon class="avatar mat-icon material-icons"  role="img" aria-hidden="true">description</mat-icon>
                                                    </div>
                                                    <div class="font-lg text-center mb-4">Workflow 2</div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" ng-if="archive">
                        <div class="col-sm-6 col-md-3">
                            <div class="ds-card ds-card-widget ds-object card-height-fix-sm clickable actionable info">
                                <div class="ds-card-container"> 
                                    <div class="ds-card-header d-block align-y-top">
                                        <div class="ds-card-subtitle">
                                            <div  class="split-content">
                                                <div class="top">
                                                    <div class="object-card-avatar">
                                                        <mat-icon class="avatar mat-icon material-icons"  role="img" aria-hidden="true">description</mat-icon>
                                                    </div>
                                                    <div class="font-lg text-center mb-4">Archived Workflow</div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
```
</div>

## Inactive Content
When a content is no longer needed, but should still be kept in the site for reference. The user should still be able to view details on inactive content, but will not be edit until it's restored.  <br/>
A badge should be added to all inactive onjects, and the color should be modifyed to set it apart from active objects. When _Include Inactive_ is checked, inactive content is added back in, in the order already determined by the content.

<div class="button-bar">
    <button class="btn btn-icon" ng-click="toggleInactive = !toggleInactive">
        <i class="material-icons">code</i>
        View Code
    </button>
</div>
<div class="example-block" ng-class="{'open': toggleInactive}">
    <div class="ui mb-4">
        <div class="overflow-list">
            <div class="ds-card">
                <div class="ds-card-header header-bar border-top-primary">
                    <div class="ds-card-header-row">
                        <div class="ds-card-title">
                            <h1>Card Header</h1>
                        </div>
                        <div class="ds-card-action">
                            <div class="custom-control custom-checkbox">
                                <input type="checkbox" class="custom-control-input" id="inactive" ng-model="inactive">
                                <label class="custom-control-label" for="inactive">Include Inactive</label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="ds-card-body">
                    <div class="ds-card-content">
                        <div class="overflow-list">
                            <div class="row">
                                <div class="col-sm-6 col-md-3">
                                    <div class="ds-card ds-card-widget info">
                                        <div class="ds-card-icon align-header">
                                            <i class="material-icons">attach_money</i>
                                        </div>
                                        <div class="ds-card-container">
                                            <div class="ds-card-header">
                                                <div class="ds-card-header-row">
                                                    <div class="ds-card-title">
                                                        <div class="font-xl">Profile</div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6 col-md-3" ng-if="inactive">
                                    <div class="ds-card ds-card-widget archive">
                                        <div class="ds-card-icon align-header">
                                            <i class="material-icons">attach_money</i>
                                        </div>
                                        <div class="ds-card-container">
                                            <div class="ds-card-header">
                                                <div class="ds-card-header-row">
                                                    <div class="ds-card-title">
                                                        <div class="font-xl">Profile 2</div>
                                                    </div>
                                                    <div class="ds-card-right-content">
                                                        <span class="badge badge-pill badge-inactive">Inactive</span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6 col-md-3">
                                    <div class="ds-card ds-card-widget info">
                                        <div class="ds-card-icon align-header">
                                            <i class="material-icons">attach_money</i>
                                        </div>
                                        <div class="ds-card-container">
                                            <div class="ds-card-header">
                                                <div class="ds-card-header-row">
                                                    <div class="ds-card-title">
                                                        <div class="font-xl">Profile 3</div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

```html
<!-- Angular -->
<div class="overflow-list">
    <ds-card border="top">
        <ds-card-header>
            <ds-card-header-title>Card Header</ds-card-header-title>
            <ds-card-title-action>
                <div class="custom-control custom-checkbox">
                    <input type="checkbox" class="custom-control-input" id="inactive" ng-model="inactive">
                    <label class="custom-control-label" for="inactive">Include Inactive</label>
                </div>
            </ds-card-title-action>
        </ds-card-header>
        <ds-card-content>
            <div class="overflow-list">
                <div class="row">
                    <div class="col-sm-6 col-md-3">
                        <ds-card mode="widget-nobody" color="info">
                            <div ds-card-icon>attach_money</div>
                            <ds-card-header>
                                <ds-card-widget-title>Profile</ds-card-widget-title>
                            </ds-card-header>
                        </ds-card>
                    </div>
                    <div class="col-sm-6 col-md-3">
                        <ds-card mode="widget-nobody" color="inactive">
                            <div ds-card-icon>attach_money</div>
                            <ds-card-header>
                                <ds-card-widget-title>Profile 2</ds-card-widget-title>
                                <ds-card-right-content></ds-card-right-content>
                            </ds-card-header>
                        </ds-card>
                    </div>
                    <div class="col-sm-6 col-md-3">
                        <ds-card mode="widget-nobody" color="info">
                            <div ds-card-icon>attach_money</div>
                            <ds-card-header>
                                <ds-card-widget-title>Profile 3</ds-card-widget-title>
                            </ds-card-header>
                        </ds-card>
                    </div>
                </div>
            </div>
        </ds-card-content>
    </ds-card>
</div>

<!-- Legacy -->
<div class="overflow-list">
    <div class="ds-card">
        <div class="ds-card-header header-bar border-top-primary">
            <div class="ds-card-header-row">
                <div class="ds-card-title">
                    <h1>Card Header</h1>
                </div>
                <div class="ds-card-action">
                    <div class="custom-control custom-checkbox">
                        <input type="checkbox" class="custom-control-input" id="inactive" ng-model="inactive">
                        <label class="custom-control-label" for="inactive">Include Inactive</label>
                    </div>
                </div>
            </div>
        </div>
        <div class="ds-card-body">
            <div class="ds-card-content">
                <div class="overflow-list">
                    <div class="row">
                        <div class="col-sm-6 col-md-3">
                            <div class="ds-card ds-card-widget info">
                                <div class="ds-card-icon align-header">
                                    <i class="material-icons">attach_money</i>
                                </div>
                                <div class="ds-card-container">
                                    <div class="ds-card-header">
                                        <div class="ds-card-header-row">
                                            <div class="ds-card-title">
                                                <div class="font-xl">Profile</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-3" ng-if="inactive">
                            <div class="ds-card ds-card-widget archive">
                                <div class="ds-card-icon align-header">
                                    <i class="material-icons">attach_money</i>
                                </div>
                                <div class="ds-card-container">
                                    <div class="ds-card-header">
                                        <div class="ds-card-header-row">
                                            <div class="ds-card-title">
                                                <div class="font-xl text-truncate">Profile 2</div>
                                            </div>
                                            <div class="ds-card-right-content">
                                                <span class="badge badge-pill badge-inactive">Inactive</span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-3">
                            <div class="ds-card ds-card-widget info">
                                <div class="ds-card-icon align-header">
                                    <i class="material-icons">attach_money</i>
                                </div>
                                <div class="ds-card-container">
                                    <div class="ds-card-header">
                                        <div class="ds-card-header-row">
                                            <div class="ds-card-title">
                                                <div class="font-xl">Profile 3</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
```
</div>
