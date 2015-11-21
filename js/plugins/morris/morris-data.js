function LoadBarChart(chartJSONdata) {    
    // https://github.com/morrisjs/morris.js/blob/master/examples/updating.html       
    graph.setData(JSON.parse(chartJSONdata));        
}
var graph;
$(function () {

    // Bar Chart
    graph = Morris.Bar({
        element: 'morris-bar-chart', 
        xkey: 'RenderingFullName',
        ykeys: ['TimeTaken'],
        labels: ['Max Time taken(MS)'],
        //barRatio: 0.4,
        //xLabelAngle: 35,
        hideHover: 'auto',
        resize: true
    });


});
