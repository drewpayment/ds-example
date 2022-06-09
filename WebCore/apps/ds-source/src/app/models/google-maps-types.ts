

export const OverlayType = {
    CIRCLE: 'circle',
    MARKER: 'marker',
};

export interface OverlayCompleteEvent<T extends any> {
    overlay: T;
    // OverlayType
    type: string;
}

export interface ComponentRestrictions {
    country: string | string[];
}

export interface AutocompleteSessionToken {
    [key: string]: string;
}

/**
 * You may restrict results from a Place Autocomplete request to be of a certain [type]
 * by passing a types parameter. The parameter specifies a type or a type collection, as
 * listed in the supported types below. If nothing is specified, all types are returned.
 * In general only a single type is allowed. The exception is that you can safely mix the
 * geocode and establishment types, but note that this will have the same effect as
 * specifying no types. The supported types are:
 * - [geocode]
 * - [address]
 * - [establishment]
 * - [regions] - returns: [locality], [sublocality], [postal_code], [country], [administrative_area_level_1], [administrative_area_level_2]
 * - [city] - returns: [locality] or [administrative_area_level_3]
 */
export interface AutocompletionRequest {
    bounds?: any;
    componentRestrictions?: ComponentRestrictions;
    input: string;
    location?: any;
    offset?: number;
    origin?: any;
    radius?: number;
    sessionToken?: AutocompleteSessionToken;
    types?: string[];
}

/**
 * google.maps.places.AutocompletePrediction interface
 *
 * Represents a single autocomplete prediction.
 */
export interface AutocompletePrediction {
    description: string;
    distance_meters?: string;
    matched_substrings: PredictionSubstring[];
    place_id: string;
    structured_formatting: StructuredFormatting;
    terms: PredictionTerm[];
    /**
     * Array of types that the prediction belonds to, for example `establishment` or `geocode`.
     */
    types: string[];
}

/**
 * google.maps.places.PredictionSubstring interface
 *
 * Represents a prediction substring.
 */
export interface PredictionSubstring {
    length: number;
    offset: number;
}

/**
 * google.maps.places.StructuredFormatting interface
 *
 * Contains structured information about the place's description, divided into a
 * main text and a secondary text, including an array of matched substrings from
 * the autocomplete input, identified by an offset and a length,
 * expressed in unicode characters.
 */
export interface StructuredFormatting {
    main_text: string;
    main_text_matched_substrings: PredictionSubstring[];
    secondary_text: string;
}

/**
 * google.maps.places.PredictionTerm interface
 *
 * Represents a prediction term.
 */
export interface PredictionTerm {
    offset: number;
    value: number;
}

export interface AutocompletePlacePredictionsResult {
    predictions: AutocompletePrediction[];
    /**
     *
     */
    status: string; // need to add constant once I understand the syntax
}

/**
 * google.maps.places.QueryAutocompletionRequest interface
 *
 * An QueryAutocompletion request to be sent to the QueryAutocompleteService.
 */
export interface QueryAutocompletionRequest {
    bounds?: any;
    input: string;
    location?: any;
    offset?: number;
    radius?: number;
}

export interface QueryAutocompletePrediction {
    description: string;
    matched_substrings: PredictionSubstring[];
    place_id?: string;
    terms: PredictionTerm[];
}

export interface QueryAutocompletePlacePredictionsResult {
    queryPredictions: QueryAutocompletePrediction[];
    status: string;
}

export interface SearchBoxOptions {
    bounds?: any;
}

/**
 * google.maps.places.AutocompleteOptions interface
 *
 * The options that can be set on an Autocomplete object.
 */
export interface AutocompleteOptions {
    /**
     * The area in which to search for places.
     */
    bounds?: any;
    /**
     * The component restrictions. Component restrictions are used to restrict predictions to only those
     * within the parent component. For example, the country.
     */
    componentRestrictions?: ComponentRestrictions;
    /**
     * Fields to be included for the Place in the details response when the details are successfully
     * retrieved, which will be billed for. If ['ALL'] is passed in, all available fields will be
     * returned and billed for (this is not recommended for production deployments). For a list of
     * fields see PlaceResult. Nested fields can be specified with dot-paths (for example, "geometry.location").
     */
    fields?: string[];
    /**
     * A boolean value, indicating that the Autocomplete widget should only return
     * those places that are inside the bounds of the Autocomplete widget at the time
     * the query is sent. Setting strictBounds to false (which is the default) will
     * make the results biased towards, but not restricted to, places contained within the bounds.
     */
    strictBounds?: boolean;
    /**
     * The types of predictions to be returned. For a list of supported types, see the
     * developer's guide. If nothing is specified, all types are returned. In general only a single
     * type is allowed. The exception is that you can safely mix the 'geocode' and 'establishment'
     * types, but note that this will have the same effect as specifying no types.
     */
    types?: string[];
}
