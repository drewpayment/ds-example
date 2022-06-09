import { Component } from "@angular/core";

@Component({
    selector: 'performance-reviews',
    template: `
        <div class="row">
            <div class="col-12">
                <a routerLink="/performance/setup/ratings">Ratings</a><br>
                <a routerLink="/performance/setup/competencies">Competencies</a><br>
                <a routerLink="/performance/setup/competencies/models">Competency Models</a><br>
                <a routerLink="/performance/setup/feedback">Feedback</a><br>
                <a routerLink="/performance/setup/review-profiles">Review Profiles</a><br>
                <a routerLink="/performance/setup/review-policy">Review Cycles</a><br>
                <a routerLink="/performance/goals">Company Goals</a><br>
                <br>
                <a href="EmployeePerformance.aspx">Employee Performance (Reviews, Goals)</a><br>    
            </div>
        </div>
    `
})
export class PerformanceComponent {}