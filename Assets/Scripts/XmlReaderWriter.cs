using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;

public class XmlReaderWriter {

	public string annotationXmlPath;

	public XmlReaderWriter(string xmlPath){
		annotationXmlPath = xmlPath;
	}

	Take take;
	public Take Take{
		get {return take;}
	}


	public void LoadFromFile(){

		List<AnnotationText> textAnnotationList = new List<AnnotationText> ();
		List<AnnotationMark> markAnnotationList = new List<AnnotationMark> ();
		List<AnnotationInk> inkAnnotationList = new List<AnnotationInk> ();
		List<AnnotationLink> linkAnnotationList = new List<AnnotationLink>();

		if (annotationXmlPath == null) {
			Debug.Log ("ERROR - XML path null");
			return;
		}

		TextAsset textAsset = new TextAsset();
		textAsset = (TextAsset)Resources.Load(annotationXmlPath, typeof(TextAsset));

		if (textAsset == null) {
			if(annotationXmlPath.Contains(".xml")) {
			   Debug.Log ("ERROR - the filename can't have extension. Delete .xml from the filename");
			}
			else {
				Debug.Log ("ERROR - There was a problem loading xml file");
			}
			return;
		}

		XmlDocument xml = new XmlDocument ();
		xml.LoadXml (textAsset.text);



		// Load Take
		XmlNodeList xmlTakeList = xml.SelectNodes ("/annotation_document/session/take");
		for (int j = 0; j < xmlTakeList.Count; j++) {

			XmlNodeList xmlAnnotationSetList = xmlTakeList.Item(j).SelectNodes("annotations/annotation_set");
			if(xmlAnnotationSetList.Count == 0)
				continue;

			take =  CreateTake(xmlTakeList.Item(j));
			if(take == null) {
				Debug.Log("ERROR creating class Take");
				return;
			}

			// Load Annotations
			//XmlNodeList xmlAnnotationSetList = xml.SelectNodes ("/annotation_document/session/take/annotations/annotation_set");
			int nNodes = xmlAnnotationSetList.Count;
			//Debug.Log ("annotation set count: " + nNodes);
			for (int i = 0; i < nNodes; i++) {
					//Debug.Log("annotation set name: " + xmlAnnotationSetList.Item(i).Name);
					XmlNodeList tmpList = xmlAnnotationSetList.Item (i).SelectNodes ("annotation");
					//Debug.Log("annotation count: " + tmpList.Count);
					for (int w = 0; w < tmpList.Count; w++) {
							XmlNode xmlAnnotation = tmpList.Item (w);

							//	Debug.Log("type = " + xmlAnnotation.Attributes[0].InnerText);
							switch (xmlAnnotation.Attributes [0].InnerText) {
			
							case "text":
									AnnotationText annotation = CreateAnnotationText (xmlAnnotation);
									if(annotation == null)
										continue;
									
									annotation.Type = "text";
									textAnnotationList.Add (annotation);
									Debug.Log ("TEXT annotation");
									continue;
							case "mark":
									AnnotationMark annotationMark = new AnnotationMark ();
									annotationMark.Type = "mark";
									annotationMark = (AnnotationMark)AddAnnotationCommonFields (xmlAnnotation, annotationMark);
									markAnnotationList.Add (annotationMark);
									Debug.Log ("MARK annotation");
									continue;
							case "ink":
									AnnotationInk annotationInk = new AnnotationInk ();
									annotationInk.Type = "ink";
									annotationInk = CreateAnnotationInk (xmlAnnotation);
									inkAnnotationList.Add (annotationInk);
									Debug.Log ("INK annotation");
									continue;
							case "link":
									AnnotationLink annotationLink = new AnnotationLink ();
									annotationLink.Type = "link";
									annotationLink = CreateAnnotationLink (xmlAnnotation);
									linkAnnotationList.Add (annotationLink);
									Debug.Log ("LINK annotation");
									continue;
							case "audio":
									Debug.Log ("Audio annotation");
									continue;
							default:
									continue;
							}
					}
			}

			take.TextAnnotationList = textAnnotationList;
			take.InkAnnotationList = inkAnnotationList;
			take.MarkAnnotationList = markAnnotationList;
			take.LinkAnnotationList = linkAnnotationList;
			Debug.Log ("Number of Text Annotation: " + take.TextAnnotationList.Count);
			Debug.Log ("Number of Mark Annotation: " + take.MarkAnnotationList.Count);
			Debug.Log ("Number of Ink Annotation: " + take.InkAnnotationList.Count);
			Debug.Log ("Number of Link Annotation: " + take.LinkAnnotationList.Count);
		}
	}


	Take CreateTake(XmlNode xmlTake){
	
		Take take = new Take ();
		if (xmlTake.Attributes.Count < 2) {
			Debug.Log ("XML Node TAKE is missing attributes");
			return null;
		}

		take.Name = xmlTake.Attributes.Item (0).InnerText;
		take.MultiVideo = Int32.Parse(xmlTake.Attributes.Item (1).InnerText);

		return take;
	
	}

	AnnotationText CreateAnnotationText(XmlNode xmlAnnotation){

		AnnotationText annotationText = new AnnotationText ();

		XmlNode xmlElements = null;
		XmlNodeList annotationChildNodeList = xmlAnnotation.ChildNodes;
		for (int w = 0; w < annotationChildNodeList.Count; w++) {
			xmlElements = annotationChildNodeList.Item (w);
			switch (xmlElements.Name) {

			case "value":
				//Debug.Log ("value: " + xmlElements.InnerText);
				if(xmlElements.InnerText.Length == 0)
					return null;

				annotationText.TextValue = xmlElements.InnerText;
				break;
	
			case "formatting":
				//Debug.Log ("formatting: " + xmlElements.InnerText);
				XmlNodeList formattingChildNodes = xmlElements.ChildNodes;
				XmlNode xmlColor = formattingChildNodes.Item (0);

				FormattingText formattingText = new FormattingText ();
				int red = Int32.Parse (formattingChildNodes.Item (0).ChildNodes.Item (0).InnerText);
				int green = Int32.Parse (formattingChildNodes.Item (0).ChildNodes.Item (1).InnerText);
				int blue = Int32.Parse (formattingChildNodes.Item (0).ChildNodes.Item (2).InnerText);
				formattingText.Color = new Color32 ((byte)red, (byte)green, (byte)blue, 0);

				annotationText.FormattingText = formattingText;

				break;

			default:
				break;
			}
		}

		annotationText = (AnnotationText) AddAnnotationCommonFields (xmlAnnotation, annotationText);

		return annotationText;
	}
	

	AnnotationInk CreateAnnotationInk(XmlNode xmlAnnotation){
	
		AnnotationInk annotationInk = new AnnotationInk ();
		
		XmlNode xmlElements = null;
		XmlNodeList annotationChildNodeList = xmlAnnotation.ChildNodes;
		for (int w = 0; w < annotationChildNodeList.Count; w++) {
			xmlElements = annotationChildNodeList.Item (w);
			switch (xmlElements.Name) {
				
			case "formatting":
				//Debug.Log ("formatting: " + xmlElements.InnerText);
				XmlNodeList formattingChildNodes = xmlElements.ChildNodes;
				XmlNode xmlColor = formattingChildNodes.Item (0);
				
				FormattingInk formattingInk = new FormattingInk ();
				int red = Int32.Parse (formattingChildNodes.Item (0).ChildNodes.Item (0).InnerText);
				int green = Int32.Parse (formattingChildNodes.Item (0).ChildNodes.Item (1).InnerText);
				int blue = Int32.Parse (formattingChildNodes.Item (0).ChildNodes.Item (2).InnerText);
				formattingInk.Color = new Color32 ((byte)red, (byte)green, (byte)blue, 0);


				XmlNode xmlThickness = formattingChildNodes.Item (1);
				formattingInk.Thickness = Int32.Parse(xmlThickness.InnerText);
				annotationInk.FormattingInk = formattingInk;
				
				break;

			case "path":
				XmlNodeList pathXmlList = xmlElements.ChildNodes;
				List<Vector2> pathList = new List<Vector2>();
				int x = 0;
				for (int j = 0; j < pathXmlList.Count; j++) {
					if(pathXmlList.Item (j).Name.Equals("x")) {
						x = Int32.Parse(pathXmlList.Item (j).InnerText);
					}
					if(pathXmlList.Item (j).Name.Equals("y")) {
						Vector2 position = new Vector2();
						position.x = x;
						position.y = Int32.Parse(pathXmlList.Item (j).InnerText);

						pathList.Add(position);
					}
				}
				annotationInk.Paths = pathList;
				
				break;

			default:
				break;
			}
		}
		
		annotationInk = (AnnotationInk) AddAnnotationCommonFields (xmlAnnotation, annotationInk);
		
		return annotationInk;
	}

	AnnotationLink CreateAnnotationLink(XmlNode xmlAnnotation){

		AnnotationLink annotationLink = new AnnotationLink ();
		
		XmlNode xmlElements = null;
		XmlNodeList annotationChildNodeList = xmlAnnotation.ChildNodes;
		for (int w = 0; w < annotationChildNodeList.Count; w++) {
			xmlElements = annotationChildNodeList.Item (w);
			switch (xmlElements.Name) {                                   
				
			case "value":
				//Debug.Log ("value: " + xmlElements.InnerText);
				annotationLink.Link = xmlElements.InnerText;
				break;
		
			default:
				break;
			}
		}
		
		annotationLink = (AnnotationLink) AddAnnotationCommonFields (xmlAnnotation, annotationLink);
		
		return annotationLink;

	}

	Annotation AddAnnotationCommonFields(XmlNode xmlAnnotation, Annotation annotation){

		XmlNodeList annotationChildNodeList = xmlAnnotation.ChildNodes;
		for (int w = 0; w < annotationChildNodeList.Count; w++) {
			XmlNode xmlElements = annotationChildNodeList.Item (w);
			switch (xmlElements.Name) {
	
			case "id":
				//Debug.Log ("ID: " + xmlElements.InnerText);
				annotation.ID = Int32.Parse (xmlElements.InnerText);
				break;
	
			case "begin":
				//Debug.Log ("begin: " + xmlElements.InnerText);
				annotation.begin = Int32.Parse (xmlElements.InnerText);
				break;
	
			case "end":
				//Debug.Log ("end: " + xmlElements.InnerText);
				annotation.end = Int32.Parse (xmlElements.InnerText);
				break;
	
			case "timeoutvisualizing":
				//Debug.Log ("timeoutvisualizing: " + xmlElements.InnerText);
				annotation.timeoutVisualizing = Int32.Parse (xmlElements.InnerText);
				break;
	
			case "timeoutfading":
				//Debug.Log ("timeoutfading: " + xmlElements.InnerText);
				annotation.timeoutFading = Int32.Parse (xmlElements.InnerText);
				break;
	
			case "positionKin": // positionKin
				//Debug.Log ("position: " + xmlElements.InnerText);
				XmlNodeList positionChildNodes = xmlElements.ChildNodes;
				Vector3 position = new Vector3 ();
				position.x = float.Parse (positionChildNodes.Item (0).InnerText);
				//Debug.Log ("position X: " + position.x);
				position.y = float.Parse (positionChildNodes.Item (1).InnerText);
				//Debug.Log ("position Y: " + position.y);
				position.z= float.Parse (positionChildNodes.Item (2).InnerText);
				//Debug.Log ("position Z: " + position.z);

				annotation.PositionKin = position;
				break;
	
			default:
					break;
			}
		}
		return annotation;
	}

}

public class AnnotationDocument {

	public MetaData metaData;
	public Marks marks;
	public Session session;
}

public class MetaData {

	public string author;
	public string date;
}

public class Marks {

	public Mark[] markList;
}

public class Mark {
	
	public int id;
	public string name;
	public string path;
	public Color color;
}

public class Session {
	
	public string name;
	public string date;
	public Take take;
	
}
	
public class Take {
	
	public string name;
	public string Name{
		get {return name;}
		set {name = value;}
	}
	public int multiVideo;
	public int MultiVideo{
		get {return multiVideo;}
		set {multiVideo = value;}
	}

	//public string path;


	List<AnnotationText> textAnnotationList;
	public List<AnnotationText> TextAnnotationList {
		get {return textAnnotationList;}
		set {textAnnotationList = value;}
	}
	
	List<AnnotationMark> markAnnotationList; 
	public List<AnnotationMark> MarkAnnotationList {
		get {return markAnnotationList;}
		set {markAnnotationList = value;}
	}
	
	List<AnnotationInk> inkAnnotationList;
	public List<AnnotationInk> InkAnnotationList {
		get {return inkAnnotationList;}
		set {inkAnnotationList = value;}
	}
	
	List<AnnotationLink> linkAnnotationList;
	public List<AnnotationLink> LinkAnnotationList {
		get {return linkAnnotationList;}
		set {linkAnnotationList = value;}
	}
}


public class Annotations {
	
	public AnnotationSet[] annotationSet;
}

public class AnnotationSet {
	
	public int id;
	public int begin;
	public int end;
	public int timeoutVisualizing;
	public int timeoutFading;
	public Annotation annotation;
}

public class Annotation {
	
	public int id;
	public int ID{
		get {return id;}
		set {id = value;}
	}

	public string type;
	public string Type {
		get {return type;}
		set {type = value;}
	}

	public int begin;
	public int Begin {
		get {return begin;}
		set {begin = value;}
	}

	public int end;
	public int End {
		get {return end;}
		set {end = value;}
	}

	//public int camera; // ignore
	public int timeoutVisualizing;
	public int TimeoutVisualizing {
		get {return timeoutVisualizing;}
		set {timeoutVisualizing = value;}
	}

	public int timeoutFading;
	public int TimeoutFading {
		get {return timeoutFading;}
		set {timeoutFading = value;}
	}

	public int mode;
	public int Mode {
		get {return mode;}
		set {mode = value;}
	}

	public Vector3 positionKin;
	public Vector3 PositionKin {
		get {return positionKin;}
		set {positionKin = value;}
	} 
}


public class AnnotationMark : Annotation {
	
	//public SceneSize sceneSize;
}

public class AnnotationText : Annotation {

	public string textValue;
	public string TextValue {
		get {return textValue;}
		set {textValue = value;}
	}

	//public SceneSize sceneSize;

	public FormattingText formattingText;
	public FormattingText FormattingText {
		get {return formattingText;}
		set {formattingText =  value;}
	}
}

public class AnnotationLink : Annotation {

	public string link;
	public string Link {
		get {return link;}
		set {link =  value;}
	}
	//public SceneSize sceneSize;
}

public class AnnotationInk : Annotation {


	public FormattingInk formattingInk;
	public FormattingInk FormattingInk {
		get {return formattingInk;}
		set {formattingInk = value;}
	}

	//public SceneSize sceneSize;

	public List<Vector2> paths;
	public List<Vector2> Paths {
		get {return paths;}
		set {paths = value;}
	}
}

public class FormattingInk {
	
	public Color32 color;
	public Color32 Color {
		get {return color;}
		set {color = value;}
	}

	public int thickness;
	public int Thickness {
		get {return thickness;}
		set {thickness = value;}
	}
}

public class FormattingText {
	
	public Color32 color;
	public Color32 Color {
		get {return color;}
		set {color = value;}
	}

	public AnnotationFont font;
	public AnnotationFont Font {
		get {return font;}
		set {font = value;}
	}
}

public class SceneSize {

	public int width;
	public int height;
}

public class AnnotationFont {

	public string family;
	public string Family {
		get {return family;}
		set {family = value;}
	}

	public int size;
	public int Size {
		get {return size;}
		set {size = value;}
	}

	public bool bold;
	public bool Bold {
		get {return bold;}
		set {bold = value;}
	}

	public bool italic;
	public bool Italic {
		get {return italic;}
		set {italic = value;}
	}

	public bool underline;
	public bool Underline {
		get {return underline;}
		set {underline = value;}
	}
	
	public bool strikeout;
	public bool Strikeout {
		get {return strikeout;}
		set {strikeout = value;}
	}
}
	