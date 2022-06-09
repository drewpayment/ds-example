import { Component, OnInit } from '@angular/core';
import { UserInfo } from '@ds/core/shared/user-info.model';
import { IVendorMaintenanceInfo, IBankDepositInfo, IAddressInfo } from '../shared/index';
import { ICountry, IState } from '@ajs/applicantTracking/shared/models';
import { ITaxFrequency } from '../shared/tax-frequency-list.model';
import { LocationApiService } from '@ds/core/location';
import { VendorMaintenanceService } from '../shared/vendor-maintenance.service';
import { zip } from 'rxjs';
import { map } from 'rxjs/operators';
import { BanksApiService, IBankInfo } from '@ds/core/banks';
import { TaxFrequencyService } from '../shared/tax-frequency.service';
import { NgModel, NgForm } from '@angular/forms';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';

@Component({
  selector: 'ds-vendor-maintenance',
  templateUrl: './vendor-maintenance.component.html',
  styleUrls: ['./vendor-maintenance.component.scss']
})
export class VendorMaintenanceComponent implements OnInit {

  user  :UserInfo;
  isLoading              : boolean = true;
  selectedVendor         : IVendorMaintenanceInfo;
  selectedVendorId       : number;
  selectedFrequency      : ITaxFrequency;
  selectedBank           : IBankInfo;
  selectedCountry        : ICountry;
  selectedState          : IState;
  vendors                : IVendorMaintenanceInfo[];
  frequencies            : ITaxFrequency[];
  banks                  : IBankInfo[];
  countries              : ICountry[];
  states                 : IState[];
  VendorMaintenanceInfo  : IVendorMaintenanceInfo;


  constructor(
    private locationApiService: LocationApiService, 
    private vendorApiService: VendorMaintenanceService,
    private bankApiService: BanksApiService,
    private frequencyApiService: TaxFrequencyService,
    private msg:DsMsgService
  ) { }

  ngOnInit() {
    this.selectedVendor = <IVendorMaintenanceInfo>{};
    this.selectedVendorId = null;
    this.selectedBank = <IBankInfo>{};
    this.selectedFrequency = <ITaxFrequency>{};      
    this.loadDropDowns();
  }

  loadDropDowns() {
    let vendors$ = this.vendorApiService.getVendors();
    let countries$ = this.locationApiService.getCountryList();
    let states$ = this.locationApiService.getStatesByCountry(this.locationApiService.defaults.countries.usa);
    let banks$ = this.bankApiService.getBanks();
    let frequencies$ = this.frequencyApiService.getTaxFrequency();

    zip(vendors$, countries$, states$, banks$, frequencies$)
      .pipe(map(results => { return { vendors: results[0], countries: results[1], states: results[2], banks: results[3], frequencies: results[4] } }))
      .subscribe(results => {
        this.vendors = results.vendors.sort((v1,v2) => {
          return v1.name > v2.name ? 1 : -1;
        });
        this.countries = results.countries;
        this.states = results.states;
        this.frequencies = results.frequencies;
        this.banks = results.banks.sort((b1,b2) => {
            return b1.name > b2.name ? 1 : -1;
        });        

        this.selectedCountry = results.countries.find(c => c.countryId === this.locationApiService.defaults.countries.usa);
        
        this.isLoading = false;
      }); 
  }

  vendorChange() {
    this.selectedVendor = this.vendors.find(v => v.vendorId === this.selectedVendorId) || <IVendorMaintenanceInfo>{};
    if(this.selectedCountry.countryId !== this.selectedVendor.countryId) {
      this.selectedCountry = this.countries.find(c => c.countryId === this.selectedVendor.countryId);
      if(!this.selectedCountry) {
        this.selectedCountry = this.countries.find(c => c.countryId === this.locationApiService.defaults.countries.usa);
      }
    }
    this.updateCountryState();        
  }

  updateCountryState() {
    if(this.selectedCountry === null) {
      this.selectedCountry = this.countries.find(c => c.countryId === this.locationApiService.defaults.countries.usa);
    }
    this.selectedVendor.countryId = this.selectedCountry.countryId;
    this.locationApiService.getStatesByCountry(this.selectedCountry.countryId)
        .subscribe(states => {
          this.states = states;
          this.selectedState = null;
        });
  }

  saveClick() {
    this.selectedVendor.countryId = this.selectedCountry.countryId;
    this.vendorApiService.saveVendor(this.selectedVendor)
      .subscribe((vendor) => {
        this.msg.setTemporarySuccessMessage('Successfully saved vendor.', 5000);
        this.vendorApiService.getVendors().subscribe(data => {
          this.vendors = data;
          this.selectedVendor = this.vendors.find(v => v.vendorId === vendor.vendorId);
          this.selectedVendorId = vendor.vendorId;
        });
      });
      
  }

  deleteClick(form: NgForm) {    
    this.vendorApiService.deleteVendor(this.selectedVendorId)
    .subscribe(() => {
      this.msg.setTemporarySuccessMessage('Successfully deleted vendor.', 5000);
      this.vendorApiService.getVendors().subscribe(data => {
        this.vendors = data;
        this.selectedVendorId = null;
        this.loadDropDowns();
        form.resetForm();               
      });
    });    
  }
}
