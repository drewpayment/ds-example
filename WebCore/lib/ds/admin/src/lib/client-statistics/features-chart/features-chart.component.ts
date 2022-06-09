import { FeaturesChartModalComponent } from './../features-chart-modal/features-chart-modal.component';
import { MatDialogConfig, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Component, OnInit } from '@angular/core';
import { Client } from '../shared/models/client';
import { ClientStatisticsApiService } from '../shared/client-statistics-api.service';
import { Features } from '../shared/models/featureEnum';
import { ExtraFeatures } from '../shared/models/extraFeaturesEnum';
import { IFeaturesChartDialogData } from '../shared/models/IFeaturesChartDialogData';

@Component({
  selector: 'ds-features-chart',
  templateUrl: './features-chart.component.html',
  styleUrls: ['./features-chart.component.scss']
})
export class FeaturesChartComponent implements OnInit {
   //Bar Chart Variables
    barChartOptions = {
        scaleShowVerticalLines: false,
        responsive: true,
        scales: {
            yAxes: [{
                barThickness: 25,
                maxBarThickness: 40,
                gridLines: {
                    display:false
                }
            }],
            xAxes: [{
                ticks: {
                    beginAtZero: true
                },
                gridLines: {
                    display:false
                }
            }]
            },
        legend: {
            display: false
        }
    };

    barColors = [{
        backgroundColor: [
            'rgb(162, 2, 156)',
            'rgb(105,44,137)',
            'rgb(62,42,139)',
            'rgb(204,164,236)',
            'rgb(126,82,174)',
            'rgb(90,77,158)',
            'rgb(141,54,139)',
            'rgb(87,14,110)',
            'rgb(12,14,110)',
        ]
    }];

    barChartLabels = ['Time & Attendance', 'Applicant Tracking', 'Time Off', 'Performance Reviews', 'Onboarding', 'ACA', 'Benefit Administration', 'Work Number'];
    chartArray = [Features.TimeClock, Features.ApplicantTracking, ExtraFeatures.LeaveManagement, Features.PerformanceReviews, Features.OnBoarding, Features.AcaReporting, Features.Benefit_Portal, ExtraFeatures.WorkNumber]

    barChartLegend = false;

    barChartData = [
        {data: [0, 0, 0, 0, 0, 0, 0, 0]}
    ];

    features = new Map<number, number>();

    clientList = new Map<number, Client[]>();

    clients: Client[];

    constructor(private apiService: ClientStatisticsApiService, private dialog: MatDialog) {
    }
    ngOnInit() {
        //Leave Management Feature Count
        this.features.set(ExtraFeatures.LeaveManagement, 0);

        this.apiService.getTotalActiveClients().subscribe((data) => {
            this.clients = data;

            data.forEach(client => {
                client.accountFeatures.forEach(feature => {
                    var tempClients = [];

                    if (this.clientList.has(Number(feature.accountFeature))){
                        tempClients = this.clientList.get(Number(feature.accountFeature));
                    }

                    tempClients.push(client);
                    this.clientList.set(Number(feature.accountFeature), tempClients);

                    if(!this.features.has(Number(feature.accountFeature))){
                        this.features.set(Number(feature.accountFeature), 1);
                    }else{
                        this.features.set(Number(feature.accountFeature), this.features.get(Number(feature.accountFeature)) + 1)
                    }
                });

                if(client.hasLeaveManagement){
                    var tempLM = [];
                    this.features.set(ExtraFeatures.LeaveManagement, this.features.get(ExtraFeatures.LeaveManagement) + 1)

                    if (this.clientList.has(ExtraFeatures.LeaveManagement)){
                        tempLM = this.clientList.get(ExtraFeatures.LeaveManagement);
                    }

                    tempLM.push(client);
                    this.clientList.set(ExtraFeatures.LeaveManagement, tempLM);
                }

                if(client.hasUnemploymentSetup){
                    var tempClients = [];

                    if (this.clientList.has(Number(ExtraFeatures.WorkNumber))){
                        tempClients = this.clientList.get(Number(ExtraFeatures.WorkNumber));
                    }

                    tempClients.push(client);
                    this.clientList.set(Number(ExtraFeatures.WorkNumber), tempClients);

                    if(!this.features.has(Number(ExtraFeatures.WorkNumber))){
                        this.features.set(Number(ExtraFeatures.WorkNumber), 1);
                    }else{
                        this.features.set(Number(ExtraFeatures.WorkNumber), this.features.get(Number(ExtraFeatures.WorkNumber)) + 1)
                    }
                }
            });

            this.barChartData = [{data: [this.features.get(Features.TimeClock), this.features.get(Features.ApplicantTracking), this.features.get(ExtraFeatures.LeaveManagement), this.features.get(Features.PerformanceReviews), this.features.get(Features.OnBoarding), this.features.get(Features.AcaReporting), this.features.get(Features.Benefit_Portal), this.features.get(ExtraFeatures.WorkNumber)]}];
            if(this.features.has(Number(Features.Geofencing))){
                this.barChartData[0].data.push(this.features.get(Number(Features.Geofencing)));
                this.barChartLabels.push('Geofencing');
                this.chartArray.push(Features.Geofencing);
            }
        });
    }

    open(event){
        if (event.active[0] != undefined){
            let config = new MatDialogConfig<IFeaturesChartDialogData>();

            config.data = {
                clientList: this.clientList.get(this.chartArray[event.active[0]._index]),
                featureName: event.active[0]._model.label
            };

            config.width = '1250px';

            return this.dialog.open<FeaturesChartModalComponent, IFeaturesChartDialogData, null>(FeaturesChartModalComponent, config);
        }
    }
    
    getTotalActiveClients() {
        return this.clients && this.clients.length >= 0
            ? this.clients.length
            : '...';
    }
    
}
