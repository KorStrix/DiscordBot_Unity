using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Strix
{
	public class BotLibrary
	{
		public class CXMLParser
		{
			public class SConfig
			{
				[XmlElement("Token")]
				public string strBotToken;

				[XmlElement( "CallID" )]
				public string strCall_ID;

				[XmlElement( "CallChannel" )]
				public string strCall_Channel;

				[XmlRoot( "Tutorial" )]
				public class STutorial
				{
					[XmlAttribute( "Title" )]
					public string strTitle;

					[XmlArray( "Fields" )]
					[XmlArrayItem( "Field" )]
					public STutorial_Field[] arrField;
				}

				public class STutorial_Field
				{
					public STutorial_Field() { }

					public STutorial_Field( string strFieldName, string strFieldValue )
					{
						this.strFieldName = strFieldName;
						this.strFieldValue = strFieldValue;
					}

					[XmlAttribute("FieldName")]
					public string strFieldName;
					[XmlAttribute( "FieldValue" )]
					public string strFieldValue;
				}


				static public SConfig LoadXML( string strFileName )
				{
					var serializer = new XmlSerializer( typeof( SConfig ) );
					using (var stream = new FileStream( Directory.GetCurrentDirectory() + "//" + strFileName, FileMode.Open ))
					{
						return serializer.Deserialize( stream ) as SConfig;
					}
				}

				static public void Save( string strFileName )
				{
					var serializer = new XmlSerializer( typeof( SConfig ) );
					using (var stream = new FileStream( strFileName, FileMode.Create ))
					{
						serializer.Serialize( stream, pConfig );
					}
				}

				[XmlElement( ElementName = "Tutorial" )]
				public STutorial pTutorial = new STutorial();
			}

			static public SConfig pConfig;

			static public SConfig Load()
			{
				pConfig = SConfig.LoadXML( "Config.xml" );
				return pConfig;
			}
		}

		static public async Task DoStartTutorial( CommandContext pContext )
		{
			var pConfig = Strix.BotLibrary.CXMLParser.Load();
			var pTutorial = pConfig.pTutorial;

			DiscordEmbedBuilder pEmbedBuilder = new DiscordEmbedBuilder();
			pEmbedBuilder.WithAuthor( pTutorial.strTitle );

			for(int i = 0; i < pTutorial.arrField.Length; i++)
				pEmbedBuilder.AddField( pTutorial.arrField[i].strFieldName, pTutorial.arrField[i].strFieldValue );

			await pContext.RespondAsync( null, false, pEmbedBuilder );
		}

		static public bool CheckIsRespond( CommandContext pContext )
		{
			return pContext.Channel.Name.ToLower().Contains( Strix.BotLibrary.CXMLParser.pConfig.strCall_Channel );
		}
	}
}
