import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResourcesListOutletComponent } from './resources-list-outlet.component';

describe('ResourcesListOutletComponent', () => {
  let component: ResourcesListOutletComponent;
  let fixture: ComponentFixture<ResourcesListOutletComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ResourcesListOutletComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResourcesListOutletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
