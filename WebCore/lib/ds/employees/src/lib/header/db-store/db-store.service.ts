import { Injectable } from '@angular/core';
import { DsStorageService } from '@ds/core/storage/storage.service';
import { EmployeeSearchOptions } from '@ds/employees/search/shared/models/employee-search-options';
import { DBConfig, NgxIndexedDBService } from 'ngx-indexed-db';
import { EMPTY, Observable, of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';


@Injectable()
export class DbStoreService {

  private static storeName = 'session_cache';
  private readonly key = 'efs';

  constructor(private db: NgxIndexedDBService,) {}

  getAll(): Observable<any> {
    return this.db.getByKey(DbStoreService.storeName, this.key);
  }

  getClientSession(clientId: number): Observable<any> {
    return this.db.getByKey(DbStoreService.storeName, this.key)
      .pipe(
        map(all => {
        const hasKeyIndex: any = all != null && (Object.keys(all).find(x => (x as any) == clientId) as any) > -1;
        if (hasKeyIndex) {
          return all[clientId];
        }
        return null;
      }));
  }

  insertOrUpdate(opts: EmployeeSearchOptions, userId: number, clientId: number): Observable<any> {
    return this.getClientSession(clientId)
      .pipe(
        catchError(err => {
          return of(null);
        }),
        switchMap(item => {
          const selectedFilters = opts.filters && opts.filters.length
            ? opts.filters.filter(x => !!x.$selected)
            : [];
          const filters = new Array(opts.filters.length);

          selectedFilters.forEach(f => {
            filters[f.filterType] = [+f.$selected.id];
          });

          const dto = {};
          dto[clientId] = {
            userId,
            sortOrder: opts.sortDirection,
            isActiveOnly: opts.isActiveOnly,
            isExcludeTemps: opts.isExcludeTemps,
            clientId,
            filters,
          };

          if (item) {
            return this.db.updateByKey(DbStoreService.storeName, dto, this.key);
          } else {
            return this.db.addItem(DbStoreService.storeName, dto, this.key);
          }
        }),
        catchError(err => {
          console.error('It tried to update something ')
          return EMPTY;
        }),
      );
  }

  getAndMergeSearchOptions(source: EmployeeSearchOptions, clientId: number): Observable<EmployeeSearchOptions> {
    return this.db.getByKey(DbStoreService.storeName, this.key)
      .pipe(map(index => {
        const result: EmployeeSearchOptions = {...source};
        const hasKeyIndex = index != null && (Object.keys(index).find(x => (x as any) == clientId) as any) > -1;
        const cached = hasKeyIndex ? index[clientId] : null;

        if (typeof cached === 'object' && cached !== null) {
          if (cached.isActiveOnly !== null)
            result.isActiveOnly = cached.isActiveOnly;

          if (cached.isExcludeTemps !== null)
            result.isExcludeTemps = cached.isExcludeTemps;

          if (cached.sortOrder !== null)
            result.sortBy = cached.sortOrder;

          if (cached.filters && cached.filters.length) {
            result.filters.forEach(filter => {
              const cf = cached.filters[filter.filterType];

              if (cf != null && cf > -1) {
                const filterOption = filter.filterOptions.find(x => x.id == cf);

                if (filterOption) {
                  filter.$selected = {
                    filterType: filterOption.filterType,
                    id: cf,
                    name: filterOption.name,
                    parentOption: null,
                  };
                }
              }
            });
          }
        }

        return result;
      }));
  }

  static configFactory(): DBConfig {
    return {
      name: 'ds',
      version: 2,
      objectStoresMeta: [{
        store: DbStoreService.storeName,
        storeConfig: {
          keyPath: 'efs',
          autoIncrement: false,
        },
        storeSchema: [
          // { name: 'employeeId', keypath: 'employeeId', options: { unique: true}},
          // { name: 'userId', keypath: ['employeeId', 'userId'], options: { unique: false } },
          // { name: 'sortOrder', keypath: 'sortOrder', options: { unique: false } },
          // { name: 'isActiveOnly', keypath: 'isActiveOnly', options: {unique: false} },
          // { name: 'isExcludeTemps', keypath: 'isExcludeTemps', options: {unique: false}},
          // { name: 'clientId', keypath: 'clientId', options: {unique: false}},
          // { name: 'filters', keypath: 'filters', options: {unique: false}},
        ],
      }],
    };
  }


}
