import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DsContactAutocompleteComponent } from './ds-contact-autocomplete.component';

describe('DsAutocompleteComponent', () => {
  let component: DsContactAutocompleteComponent;
  let fixture: ComponentFixture<DsContactAutocompleteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DsContactAutocompleteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DsContactAutocompleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
