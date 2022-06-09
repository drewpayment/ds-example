import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientGlGlobalMappingComponent } from './client-gl-global-mapping.component';

describe('ClientGlGlobalMappingComponent', () => {
  let component: ClientGlGlobalMappingComponent;
  let fixture: ComponentFixture<ClientGlGlobalMappingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientGlGlobalMappingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientGlGlobalMappingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
