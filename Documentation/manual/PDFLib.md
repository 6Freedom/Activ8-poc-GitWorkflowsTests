# PDF Parser lib comparaison

Note : On dirait que la plupart des libs sont payantes/demande de payer si on veut les utiliser pour une raison commerciale

##Contexte

Trouver une lib pour pouvoir extraire les process et les transformer ou au moins en deviner une gamme.   

## iText7 ([lien](https://github.com/itext/itext7-dotnet))

*Note : iText7 est la nouvelle version de [iTextSharp](https://www.nuget.org/packages/iTextSharp/) que Grégoire avait trouvé.*  

N'a pas l'air de proposer des services avancés, mais permettrai de lire des documents pdf en C#. Semble complexe à utiliser avec une [documentation](https://api.itextpdf.com/iText7/dotnet/7.1.8/index.html) peu expliqué (seulement de la doc généré automatiquement).
Semble proposer de convertir d'html vers pdf mais pas l'inverse.

## PdfTron ([lien](https://www.pdftron.com/))

Semble proposer un service de lecture avancé de document PDF :  
*"PDFTron.ai is a document understanding framework that combines the latest in deep learning and AI with 20 years of PDF and document expertise. PDFTron’s 3rd generation of content extraction technology is currently in development."*  
Après un test sur leur [site](https://www.pdftron.com/pdf-tools/pdf-table-extraction/) avec le document de Poclain, cet outil n'a pas l'air d'être assez fonctionnel pour nos besoins. à tester avec d'autres documents.

La conversion en HTML semble très propre après un test sur leur [site](https://www.pdftron.com/pdf-sdk/pdf-to-html/)

## Docotic.pdf de BitMiracle ([lien 1](https://bitmiracle.com/pdf-library/)) ([lien 2](https://github.com/BitMiracle/Docotic.Pdf.Samples))

A une fonction pour extraire automatiquement tout le texte et le mettre en String. Pareil pour les images. -> semble simple à utiliser.  
à l'air d'être très complet avec une bonne documentation : https://github.com/BitMiracle/Docotic.Pdf.Samples
Impossible de convertir en XML / HTML mais propose de l'OCR pour lire du texte sur des images.


## Aspose.pdf ([lien](https://products.aspose.com/pdf/net))

Permettrait de convertir le document pdf en xml ou html. Propose beaucoup de service pour modifier un pdf, mais pas pour le lire.


## XpdfReader ([lien](http://www.xpdfreader.com/index.html))

N'est pas une lib mais un logiciel capable d'ouvrir des pdfs et de les convertir.


## pdfminer.six ([lien 1](https://pdfminersix.readthedocs.io/en/latest/)) ([lien 2](https://pypi.org/project/pdfminer/))

Est en python. Mais possède un outil pour [extraire la structure du document](https://pdfminersix.readthedocs.io/en/latest/reference/commandline.html#api-dumppdf).


## PDF Reference ([lien](https://www.adobe.com/devnet/pdf/pdf_reference_archive.html))

Semble outdated : "*(Updated Oct. 23, 2007)*"


## PDF Focus ([lien](https://sautinsoft.com/products/pdf-focus/how-to-extract-text-from-pdf-in-dotnet-aspnet-csharp-vb.php))

Semble permettre de [convertir le document en xml](https://sautinsoft.com/products/pdf-focus/convert-pdf-to-xml-document-in-dotnet.php) ou en [html](https://sautinsoft.com/products/pdf-focus/convert-pdf-to-html-in-dotnet.php) et ainsi manipuler le pdf avec sa structure.
La conversion xml semble plus facile à manipuler que la conversion html. La conversion HTML laisse beaucoup plus à désirer que celle de PDFTron.

# Réflexions

Est-ce que convertir le pdf en html ou xml permettrait de plus facilement le manipuler/lire ?

Avoir une version html/xml permettrait d'avoir le document mise en page tout en gardant la structure de donnée derrière.
Le XML semble plus adapté après avoir testé les 2 façons de faire avec la lib de Sautinsoft.


Faire que l'utilisateur capture des zones de son écran pour les lire ensuite avec de l'OCR
