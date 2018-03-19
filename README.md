# fuzzlib
fuzzlib template engine. productive, lightweight, flexible
## template
```javascript
<table border="0" cellpadding="0" cellspacing="0" width="610" align="center">
    <font size="5" style="font-size: 20px" face="arial, sans-serif" color="#834dad"> User name: {%username%} </font><br/>
    <font size="5" style="font-size: 20px" face="arial, sans-serif" color="#834dad"> order id: {%orderid%} </font><br/>
    <font size="5" style="font-size: 20px" face="arial, sans-serif" color="#834dad"> order price: {%sum%} </font><br/>
    <font size="5" style="font-size: 20px" face="arial, sans-serif" color="#834dad"> order date: {%date%} </font><br/>
    
    <div style="font-size: 20px" face="arial, sans-serif">
        <ul>
            {%for:cars%}
                <li>{%=cars.id%}</li>
                <li>{%=cars.model%}</li>
                <li>{%=cars.hp%}</li>
                <li>{%=cars.color%}</li>
                <li>{%=cars.price%}</li>
                <li>{%RemoveLink({%local%},{%=cars.id%},{%orderid%})%}</li>
            {%endfor%}
        </ul>
        <br />
        cars ids:
        <ul>
            {%for:cars%}
                <li>username:{%username%}, car id:{%=cars.id%}</li>
            {%endfor%}
        </ul>
        <br />
        {%HelloWorld()%}
    </div>
</table>
```
## data xml
```xml
<item>
    <username>Britney Spears</username>
    <orderid>123456</orderid>
    <sum>120000$</sum>
    <date>15.03.2018</date>
    <local>ru</local>
    <cars>
        <param>
            <id>1</id>
            <model>Audi</model>
            <hp>170</hp>
            <color>black</color>
            <price>65000$</price>                
        </param>
        <param>
            <id>2</id>
            <model>BMW</model>
            <hp>130</hp>
            <color>blue</color>
            <price>55000$</price>     
        </param>
    </cars>
</item>
```
```javascript
template variables
    {%username%}
    {%orderid%} 
    {%sum%} 
    {%date%} 

foor loop
    {%for:cars%}        - start loop
        {%=cars.id%}
        {%=cars.model%}
        {%=cars.hp%}
        {%=cars.color%}
        {%=cars.price%}
    {%endfor%}          - end loop

custom methods
    {%RemoveLink({%local%},{%=cars.id%},{%orderid%})%}
    {%HelloWorld()%}
    
    {%sum%}         - global variable
    {%=cars.id%}    - loop variable
```
