# Overview
The following docs describe our approach to migrating from Angular JS (1x) to Angular (6+).  

This is currently in _very_ early phases of development, so things will probably change.  Therefore, when attempting to adopt anything here please be sure to review and discuss any changes with the team to be sure current best practices are followed and new patterns can be adopted by all. 

While there is much hype over the use of the `angular-cli`, our current implementation does not use it.  This is primarily due to the fact our current build relies heavily on a custom webpack config that we currently don't know how to integrate with the CLI, and the unknowns of getting the CLI working within a .NET hosted app.

# Resources
The process we've adopted relies heavily on approaches recommended by the larger development community.  

Particularly, the following resources have been very helpful:

## ngUpgrade Docs
[Official Angular ngUpgrade Documentation](https://angular.io/guide/upgrade)

[scotch.io ngUpgrade Series](https://scotch.io/@samjulien)

[ngUpgrade Examples for Various AngularJs Items](https://blog.nrwl.io/ngupgrade-in-depth-436a52298a00)

## WebPack Docs
[Official Angular Webpack Docs](https://angular.io/guide/webpack)

[ngUpgrade Webpack Series](https://medium.com/@UpgradingAJS/baby-steps-to-webpack-for-ngupgrade-535265194852)

# Example
Below is a working example based on the `hero` examples in the [Official Angular ngUpgrade Documentation](https://angular.io/guide/upgrade).  

<span class="text-info">Blue zones</span> are managed by AngularJs (AJS) and <span class='text-danger'>Red zones</span> are managed by Angular (NGX).

View the source in the `Scripts\app\ngx-migration\demo` folder.

<div class="card border-info">
    <div class="card-header">
        {{ $ctrl.ajsTitle }}
    </div>
    <div class="card-body">
        <demo></demo>
    </div>
</div>