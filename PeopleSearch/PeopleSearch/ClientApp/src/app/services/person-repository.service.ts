import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, share } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { PeopleSearchResponse } from '../models';

@Injectable()
export class PersonRepositoryService {
  private readonly searchUrl: string;
  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string
  ) {
    this.searchUrl = baseUrl + 'person/search';
  }

  runSearch(searchText: string, delayMilliseconds: number) {
    return this.http.get<PeopleSearchResponse>(this.searchUrl, {
      params: {
        searchText: searchText,
        delayMilliseconds: `${delayMilliseconds}`
      }
    });
  }
}
