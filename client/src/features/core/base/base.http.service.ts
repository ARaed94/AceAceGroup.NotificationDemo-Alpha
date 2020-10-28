import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap, finalize } from 'rxjs/operators';

@Injectable()
export class BaseHttpService {

    constructor(private http: HttpClient) {
    }

    public get(url: string, queryString: any = null, pagination: any = null): Observable<Response> {
        url = this.getFullUrl(url);
        let options = this.buildRequestOptions(queryString, pagination);
        return this.invoke(this.http.get<Response>(url, options));
    }

    public post(url: string, body: any, queryString: any = null, pagination: any = null,
        orderBy: string = null, orderDirection: string = null): Observable<Response> {
        url = this.getFullUrl(url);
        let options = this.buildRequestOptions(queryString, pagination, orderBy, orderDirection);
        return this.invoke(this.http.post<Response>(url, body, options));
    }


    private getFullUrl(url): string {
        return "https://localhost:44310/api/" + decodeURI(url);
    }


    private buildRequestOptions(queryString: any, pagination: any = null, orderBy: string = null,
        orderDirection: string = null): {} {
        let options: any = {
            params: this.buildHttpParams(queryString, pagination, orderBy, orderDirection),
            observe: 'response',
        };
        return options;
    }

    private buildHttpParams(queryString: any, pagination: any = null, orderBy: string = null,
        orderDirection: string = null): HttpParams {
        let params = new HttpParams();

        // Return an empty object of type 'HttpParams' if criteria and pagination are null.
        if (queryString == null && pagination == null) {
            return params;
        }

        // Iterate through criteria properties and append each property with its value to build an HTTP param.
        for (let property in queryString) {
            if (Object.prototype.hasOwnProperty.call(queryString, property)) {
                params = params.set(property, queryString[property]);
                console.log(params.toString());
            }
        }

        if (pagination != null) {
            if (pagination.page != null) {
                params = params.set("page", pagination.page);
            }
            if (pagination.pageSize != null) {
                params = params.set("pageSize", pagination.pageSize);
            }
        }

        // Set order by.
        if (orderBy != null && orderBy != "") {
            params = params.set("orderBy", orderBy);
            if (orderDirection == null || orderDirection == "") {
                params = params.set("orderDirection", "ASC");
            }
            else {
                params = params.set("orderDirection", orderDirection);
            }
        }
        return params;
    }

    private invoke(httpCall: Observable<Response>): Observable<Response> {
        return httpCall.pipe(map(this.mapResponse), catchError(this.handleError<Response>()),
            tap(
                (response: Response) => {
                    this.onSuccess(response)
                },
                (error: any) => {
                    this.onError(error);
                },
                () => {
                    this.onComplete()
                }),
            finalize(() => { this.onFinnilize() }));
    }

    onFinnilize() {
    }

    onComplete() {
    }

    onError(error: any) {
    }

    onSuccess(response: any) {
        return response;
    }

    mapResponse(response: Response) {
        return response;
    }

    private handleError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {

            // TODO: send the error to remote logging infrastructure
            console.error(error); // log to console instead

            // TODO: better job of transforming error for user consumption
            //this.log(`${operation} failed: ${error.message}`);

            // Let the app keep running by returning an empty result.
            return of(result as T);
        };
    }
}
