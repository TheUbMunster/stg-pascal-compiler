(Program
	(ProgramHeading
		Program 'program'
		(Identifier
			Letter 'S'
			Letter 'c'
			Letter 'a'
			Letter 'l'
			Letter 'a'
			Letter 'r'
			Letter 's'
		)
		OpenParen '('
		(ProgramParameterList
			(IdentifierList
				(Identifier -> variable input: text
					Letter 'i'
					Letter 'n'
					Letter 'p'
					Letter 'u'
					Letter 't'
				)
				Comma ','
				(Identifier -> variable output: text
					Letter 'o'
					Letter 'u'
					Letter 't'
					Letter 'p'
					Letter 'u'
					Letter 't'
				)
			)
		)
		CloseParen ')'
	)
	Semicolon ';'
	(ProgramBlock
		(Block
			(LabelDeclarationPart)
			(ConstantDefinitionPart
				Const 'const'
				(ConstantDefinition
					(Identifier -> constant limit = 10
						Letter 'l'
						Letter 'i'
						Letter 'm'
						Letter 'i'
						Letter 't'
					)
					Equals '='
					(Constant : integer
						(UnsignedNumber : integer
							(UnsignedInteger : integer
								(DigitSequence
									Digit '1'
									Digit '0'
								)
							)
						)
					)
				)
				Semicolon ';'
				(ConstantDefinition
					(Identifier -> constant negative = -10
						Letter 'n'
						Letter 'e'
						Letter 'g'
						Letter 'a'
						Letter 't'
						Letter 'i'
						Letter 'v'
						Letter 'e'
					)
					Equals '='
					(Constant : integer
						(Sign
							Minus '-'
						)
						(ConstantIdentifier : integer
							(Identifier -> constant limit = 10
								Letter 'l'
								Letter 'i'
								Letter 'm'
								Letter 'i'
								Letter 't'
							)
						)
					)
				)
				Semicolon ';'
				(ConstantDefinition
					(Identifier -> constant pi = 3.14159
						Letter 'p'
						Letter 'i'
					)
					Equals '='
					(Constant : real
						(UnsignedNumber : real
							(UnsignedReal : real
								(DigitSequence
									Digit '3'
								)
								Dot '.'
								(FractionalPart
									(DigitSequence
										Digit '1'
										Digit '4'
										Digit '1'
										Digit '5'
										Digit '9'
									)
								)
							)
						)
					)
				)
				Semicolon ';'
				(ConstantDefinition
					(Identifier -> constant blank = ' '
						Letter 'b'
						Letter 'l'
						Letter 'a'
						Letter 'n'
						Letter 'k'
					)
					Equals '='
					(Constant : char
						CharacterString '' ''
					)
				)
				Semicolon ';'
				(ConstantDefinition
					(Identifier -> constant greeting = 'hello'
						Letter 'g'
						Letter 'r'
						Letter 'e'
						Letter 'e'
						Letter 't'
						Letter 'i'
						Letter 'n'
						Letter 'g'
					)
					Equals '='
					(Constant : packed array[1..5] of char
						CharacterString ''hello''
					)
				)
				Semicolon ';'
			)
			(TypeDefinitionPart
				Type 'type'
				(TypeDefinition
					(Identifier -> type colour
						Letter 'c'
						Letter 'o'
						Letter 'l'
						Letter 'o'
						Letter 'u'
						Letter 'r'
					)
					Equals '='
					(TypeDenoter : colour
						(NewType : colour
							(NewOrdinalType : colour
								(EnumeratedType : colour
									OpenParen '('
									(IdentifierList
										(Identifier -> constant red = red
											Letter 'r'
											Letter 'e'
											Letter 'd'
										)
										Comma ','
										(Identifier -> constant green = green
											Letter 'g'
											Letter 'r'
											Letter 'e'
											Letter 'e'
											Letter 'n'
										)
										Comma ','
										(Identifier -> constant blue = blue
											Letter 'b'
											Letter 'l'
											Letter 'u'
											Letter 'e'
										)
									)
									CloseParen ')'
								)
							)
						)
					)
				)
				Semicolon ';'
				(TypeDefinition
					(Identifier -> type small
						Letter 's'
						Letter 'm'
						Letter 'a'
						Letter 'l'
						Letter 'l'
					)
					Equals '='
					(TypeDenoter : small
						(NewType : small
							(NewOrdinalType : small
								(SubrangeType : small
									(Constant : integer
										(UnsignedNumber : integer
											(UnsignedInteger : integer
												(DigitSequence
													Digit '1'
												)
											)
										)
									)
									DoubleDot '..'
									(Constant : integer
										(ConstantIdentifier : integer
											(Identifier -> constant limit = 10
												Letter 'l'
												Letter 'i'
												Letter 'm'
												Letter 'i'
												Letter 't'
											)
										)
									)
								)
							)
						)
					)
				)
				Semicolon ';'
				(TypeDefinition
					(Identifier -> type shade
						Letter 's'
						Letter 'h'
						Letter 'a'
						Letter 'd'
						Letter 'e'
					)
					Equals '='
					(TypeDenoter : shade
						(NewType : shade
							(NewOrdinalType : shade
								(SubrangeType : shade
									(Constant : colour
										(ConstantIdentifier : colour
											(Identifier -> constant red = red
												Letter 'r'
												Letter 'e'
												Letter 'd'
											)
										)
									)
									DoubleDot '..'
									(Constant : colour
										(ConstantIdentifier : colour
											(Identifier -> constant green = green
												Letter 'g'
												Letter 'r'
												Letter 'e'
												Letter 'e'
												Letter 'n'
											)
										)
									)
								)
							)
						)
					)
				)
				Semicolon ';'
			)
			(VariableDeclarationPart
				Var 'var'
				(VariableDeclaration
					(IdentifierList
						(Identifier -> variable i: integer
							Letter 'i'
						)
						Comma ','
						(Identifier -> variable j: integer
							Letter 'j'
						)
					)
					Colon ':'
					(TypeDenoter : integer
						(TypeIdentifier : integer
							(Identifier -> type integer
								Letter 'i'
								Letter 'n'
								Letter 't'
								Letter 'e'
								Letter 'g'
								Letter 'e'
								Letter 'r'
							)
						)
					)
				)
				Semicolon ';'
				(VariableDeclaration
					(IdentifierList
						(Identifier -> variable n: small
							Letter 'n'
						)
					)
					Colon ':'
					(TypeDenoter : small
						(TypeIdentifier : small
							(Identifier -> type small
								Letter 's'
								Letter 'm'
								Letter 'a'
								Letter 'l'
								Letter 'l'
							)
						)
					)
				)
				Semicolon ';'
				(VariableDeclaration
					(IdentifierList
						(Identifier -> variable x: real
							Letter 'x'
						)
						Comma ','
						(Identifier -> variable y: real
							Letter 'y'
						)
					)
					Colon ':'
					(TypeDenoter : real
						(TypeIdentifier : real
							(Identifier -> type real
								Letter 'r'
								Letter 'e'
								Letter 'a'
								Letter 'l'
							)
						)
					)
				)
				Semicolon ';'
				(VariableDeclaration
					(IdentifierList
						(Identifier -> variable c: char
							Letter 'c'
						)
					)
					Colon ':'
					(TypeDenoter : char
						(TypeIdentifier : char
							(Identifier -> type char
								Letter 'c'
								Letter 'h'
								Letter 'a'
								Letter 'r'
							)
						)
					)
				)
				Semicolon ';'
				(VariableDeclaration
					(IdentifierList
						(Identifier -> variable b: Boolean
							Letter 'b'
						)
					)
					Colon ':'
					(TypeDenoter : Boolean
						(TypeIdentifier : Boolean
							(Identifier -> type Boolean
								Letter 'B'
								Letter 'o'
								Letter 'o'
								Letter 'l'
								Letter 'e'
								Letter 'a'
								Letter 'n'
							)
						)
					)
				)
				Semicolon ';'
				(VariableDeclaration
					(IdentifierList
						(Identifier -> variable hue: colour
							Letter 'h'
							Letter 'u'
							Letter 'e'
						)
					)
					Colon ':'
					(TypeDenoter : colour
						(TypeIdentifier : colour
							(Identifier -> type colour
								Letter 'c'
								Letter 'o'
								Letter 'l'
								Letter 'o'
								Letter 'u'
								Letter 'r'
							)
						)
					)
				)
				Semicolon ';'
				(VariableDeclaration
					(IdentifierList
						(Identifier -> variable s: shade
							Letter 's'
						)
					)
					Colon ':'
					(TypeDenoter : shade
						(TypeIdentifier : shade
							(Identifier -> type shade
								Letter 's'
								Letter 'h'
								Letter 'a'
								Letter 'd'
								Letter 'e'
							)
						)
					)
				)
				Semicolon ';'
			)
			(ProcedureAndFunctionDeclarationPart)
			(StatementPart
				(CompoundStatement
					Begin 'begin'
					(StatementSequence
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : integer
										(EntireVariable : integer
											(VariableIdentifier : integer
												(Identifier -> variable i: integer
													Letter 'i'
												)
											)
										)
									)
									Walrus ':='
									(Expression : integer
										(SimpleExpression : integer
											(Term : integer
												(Factor : integer
													(UnsignedConstant : integer
														(ConstantIdentifier : integer
															(Identifier -> constant negative = -10
																Letter 'n'
																Letter 'e'
																Letter 'g'
																Letter 'a'
																Letter 't'
																Letter 'i'
																Letter 'v'
																Letter 'e'
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : integer
										(EntireVariable : integer
											(VariableIdentifier : integer
												(Identifier -> variable j: integer
													Letter 'j'
												)
											)
										)
									)
									Walrus ':='
									(Expression : integer
										(SimpleExpression : integer
											(Term : integer
												(Factor : integer
													(UnsignedConstant : integer
														(ConstantIdentifier : integer
															(Identifier -> constant limit = 10
																Letter 'l'
																Letter 'i'
																Letter 'm'
																Letter 'i'
																Letter 't'
															)
														)
													)
												)
												(MultiplyingOperator
													Asterisk '*'
												)
												(Factor : integer
													(UnsignedConstant : integer
														(UnsignedNumber : integer
															(UnsignedInteger : integer
																(DigitSequence
																	Digit '2'
																)
															)
														)
													)
												)
											)
											(AddingOperator
												Plus '+'
											)
											(Term : integer
												(Factor : integer
													(VariableAccess : integer
														(EntireVariable : integer
															(VariableIdentifier : integer
																(Identifier -> variable i: integer
																	Letter 'i'
																)
															)
														)
													)
												)
												(MultiplyingOperator
													Div 'div'
												)
												(Factor : integer
													(UnsignedConstant : integer
														(UnsignedNumber : integer
															(UnsignedInteger : integer
																(DigitSequence
																	Digit '3'
																)
															)
														)
													)
												)
											)
											(AddingOperator
												Minus '-'
											)
											(Term : integer
												(Factor : integer
													(VariableAccess : integer
														(EntireVariable : integer
															(VariableIdentifier : integer
																(Identifier -> variable i: integer
																	Letter 'i'
																)
															)
														)
													)
												)
												(MultiplyingOperator
													Mod 'mod'
												)
												(Factor : integer
													(UnsignedConstant : integer
														(UnsignedNumber : integer
															(UnsignedInteger : integer
																(DigitSequence
																	Digit '2'
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : real
										(EntireVariable : real
											(VariableIdentifier : real
												(Identifier -> variable x: real
													Letter 'x'
												)
											)
										)
									)
									Walrus ':='
									(Expression : real
										(SimpleExpression : real
											(Term : real
												(Factor : real
													(UnsignedConstant : real
														(ConstantIdentifier : real
															(Identifier -> constant pi = 3.14159
																Letter 'p'
																Letter 'i'
															)
														)
													)
												)
												(MultiplyingOperator
													Asterisk '*'
												)
												(Factor : real
													(UnsignedConstant : real
														(UnsignedNumber : real
															(UnsignedReal : real
																(DigitSequence
																	Digit '2'
																)
																Dot '.'
																(FractionalPart
																	(DigitSequence
																		Digit '0'
																	)
																)
																Letter 'e'
																(ScaleFactor
																	(DigitSequence
																		Digit '0'
																	)
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : real
										(EntireVariable : real
											(VariableIdentifier : real
												(Identifier -> variable y: real
													Letter 'y'
												)
											)
										)
									)
									Walrus ':='
									(Expression : real
										(SimpleExpression : real
											(Term : real
												(Factor : real
													(VariableAccess : real
														(EntireVariable : real
															(VariableIdentifier : real
																(Identifier -> variable x: real
																	Letter 'x'
																)
															)
														)
													)
												)
												(MultiplyingOperator
													ForwardSlash '/'
												)
												(Factor : integer
													(UnsignedConstant : integer
														(UnsignedNumber : integer
															(UnsignedInteger : integer
																(DigitSequence
																	Digit '2'
																)
															)
														)
													)
												)
											)
											(AddingOperator
												Plus '+'
											)
											(Term : integer
												(Factor : integer
													(VariableAccess : integer
														(EntireVariable : integer
															(VariableIdentifier : integer
																(Identifier -> variable i: integer
																	Letter 'i'
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : small
										(EntireVariable : small
											(VariableIdentifier : small
												(Identifier -> variable n: small
													Letter 'n'
												)
											)
										)
									)
									Walrus ':='
									(Expression : integer
										(SimpleExpression : integer
											(Term : integer
												(Factor : integer
													(UnsignedConstant : integer
														(UnsignedNumber : integer
															(UnsignedInteger : integer
																(DigitSequence
																	Digit '5'
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : integer
										(EntireVariable : integer
											(VariableIdentifier : integer
												(Identifier -> variable i: integer
													Letter 'i'
												)
											)
										)
									)
									Walrus ':='
									(Expression : integer
										(SimpleExpression : integer
											(Term : small
												(Factor : small
													(VariableAccess : small
														(EntireVariable : small
															(VariableIdentifier : small
																(Identifier -> variable n: small
																	Letter 'n'
																)
															)
														)
													)
												)
											)
											(AddingOperator
												Plus '+'
											)
											(Term : integer
												(Factor : integer
													(UnsignedConstant : integer
														(UnsignedNumber : integer
															(UnsignedInteger : integer
																(DigitSequence
																	Digit '1'
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : char
										(EntireVariable : char
											(VariableIdentifier : char
												(Identifier -> variable c: char
													Letter 'c'
												)
											)
										)
									)
									Walrus ':='
									(Expression : char
										(SimpleExpression : char
											(Term : char
												(Factor : char
													(UnsignedConstant : char
														(ConstantIdentifier : char
															(Identifier -> constant blank = ' '
																Letter 'b'
																Letter 'l'
																Letter 'a'
																Letter 'n'
																Letter 'k'
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : Boolean
										(EntireVariable : Boolean
											(VariableIdentifier : Boolean
												(Identifier -> variable b: Boolean
													Letter 'b'
												)
											)
										)
									)
									Walrus ':='
									(Expression : Boolean
										(SimpleExpression : Boolean
											(Term : Boolean
												(Factor : Boolean
													OpenParen '('
													(Expression : Boolean
														(SimpleExpression : char
															(Term : char
																(Factor : char
																	(VariableAccess : char
																		(EntireVariable : char
																			(VariableIdentifier : char
																				(Identifier -> variable c: char
																					Letter 'c'
																				)
																			)
																		)
																	)
																)
															)
														)
														(RelationalOperator
															Equals '='
														)
														(SimpleExpression : char
															(Term : char
																(Factor : char
																	(UnsignedConstant : char
																		CharacterString '' ''
																	)
																)
															)
														)
													)
													CloseParen ')'
												)
												(MultiplyingOperator
													And 'and'
												)
												(Factor : Boolean
													Not 'not'
													(Factor : Boolean
														OpenParen '('
														(Expression : Boolean
															(SimpleExpression : integer
																(Term : integer
																	(Factor : integer
																		(VariableAccess : integer
																			(EntireVariable : integer
																				(VariableIdentifier : integer
																					(Identifier -> variable i: integer
																						Letter 'i'
																					)
																				)
																			)
																		)
																	)
																)
															)
															(RelationalOperator
																GreaterThan '>'
															)
															(SimpleExpression : integer
																(Term : integer
																	(Factor : integer
																		(VariableAccess : integer
																			(EntireVariable : integer
																				(VariableIdentifier : integer
																					(Identifier -> variable j: integer
																						Letter 'j'
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
														CloseParen ')'
													)
												)
											)
											(AddingOperator
												Or 'or'
											)
											(Term : Boolean
												(Factor : Boolean
													OpenParen '('
													(Expression : Boolean
														(SimpleExpression : real
															(Term : real
																(Factor : real
																	(VariableAccess : real
																		(EntireVariable : real
																			(VariableIdentifier : real
																				(Identifier -> variable x: real
																					Letter 'x'
																				)
																			)
																		)
																	)
																)
															)
														)
														(RelationalOperator
															LessThanOrEqual '<='
														)
														(SimpleExpression : real
															(Term : real
																(Factor : real
																	(VariableAccess : real
																		(EntireVariable : real
																			(VariableIdentifier : real
																				(Identifier -> variable y: real
																					Letter 'y'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
													CloseParen ')'
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : colour
										(EntireVariable : colour
											(VariableIdentifier : colour
												(Identifier -> variable hue: colour
													Letter 'h'
													Letter 'u'
													Letter 'e'
												)
											)
										)
									)
									Walrus ':='
									(Expression : colour
										(SimpleExpression : colour
											(Term : colour
												(Factor : colour
													(UnsignedConstant : colour
														(ConstantIdentifier : colour
															(Identifier -> constant green = green
																Letter 'g'
																Letter 'r'
																Letter 'e'
																Letter 'e'
																Letter 'n'
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : shade
										(EntireVariable : shade
											(VariableIdentifier : shade
												(Identifier -> variable s: shade
													Letter 's'
												)
											)
										)
									)
									Walrus ':='
									(Expression : colour
										(SimpleExpression : colour
											(Term : colour
												(Factor : colour
													(UnsignedConstant : colour
														(ConstantIdentifier : colour
															(Identifier -> constant red = red
																Letter 'r'
																Letter 'e'
																Letter 'd'
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(StructuredStatement
								(ConditionalStatement
									(IfStatement
										If 'if'
										(BooleanExpression : Boolean
											(Expression : Boolean
												(SimpleExpression : colour
													(Term : colour
														(Factor : colour
															(VariableAccess : colour
																(EntireVariable : colour
																	(VariableIdentifier : colour
																		(Identifier -> variable hue: colour
																			Letter 'h'
																			Letter 'u'
																			Letter 'e'
																		)
																	)
																)
															)
														)
													)
												)
												(RelationalOperator
													Equals '='
												)
												(SimpleExpression : colour
													(Term : colour
														(Factor : colour
															(UnsignedConstant : colour
																(ConstantIdentifier : colour
																	(Identifier -> constant green = green
																		Letter 'g'
																		Letter 'r'
																		Letter 'e'
																		Letter 'e'
																		Letter 'n'
																	)
																)
															)
														)
													)
												)
											)
										)
										Then 'then'
										(Statement
											(SimpleStatement
												(AssignmentStatement
													(VariableAccess : integer
														(EntireVariable : integer
															(VariableIdentifier : integer
																(Identifier -> variable i: integer
																	Letter 'i'
																)
															)
														)
													)
													Walrus ':='
													(Expression : integer
														(SimpleExpression : integer
															(Term : integer
																(Factor : integer
																	(FunctionDesignator : integer
																		(FunctionIdentifier
																			(Identifier -> function ord: integer
																				Letter 'o'
																				Letter 'r'
																				Letter 'd'
																			)
																		)
																		(ActualParameterList
																			OpenParen '('
																			(ActualParameter
																				(Expression : colour
																					(SimpleExpression : colour
																						(Term : colour
																							(Factor : colour
																								(VariableAccess : colour
																									(EntireVariable : colour
																										(VariableIdentifier : colour
																											(Identifier -> variable hue: colour
																												Letter 'h'
																												Letter 'u'
																												Letter 'e'
																											)
																										)
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																			CloseParen ')'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										(ElsePart
											Else 'else'
											(Statement
												(SimpleStatement
													(AssignmentStatement
														(VariableAccess : integer
															(EntireVariable : integer
																(VariableIdentifier : integer
																	(Identifier -> variable i: integer
																		Letter 'i'
																	)
																)
															)
														)
														Walrus ':='
														(Expression : integer
															(SimpleExpression : integer
																(Term : integer
																	(Factor : integer
																		(FunctionDesignator : integer
																			(FunctionIdentifier
																				(Identifier -> function ord: integer
																					Letter 'o'
																					Letter 'r'
																					Letter 'd'
																				)
																			)
																			(ActualParameterList
																				OpenParen '('
																				(ActualParameter
																					(Expression : colour
																						(SimpleExpression : colour
																							(Term : colour
																								(Factor : colour
																									(UnsignedConstant : colour
																										(ConstantIdentifier : colour
																											(Identifier -> constant red = red
																												Letter 'r'
																												Letter 'e'
																												Letter 'd'
																											)
																										)
																									)
																								)
																							)
																						)
																					)
																				)
																				CloseParen ')'
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(StructuredStatement
								(RepetitiveStatement
									(WhileStatement
										While 'while'
										(BooleanExpression : Boolean
											(Expression : Boolean
												(SimpleExpression : integer
													(Term : integer
														(Factor : integer
															(VariableAccess : integer
																(EntireVariable : integer
																	(VariableIdentifier : integer
																		(Identifier -> variable i: integer
																			Letter 'i'
																		)
																	)
																)
															)
														)
													)
												)
												(RelationalOperator
													LessThan '<'
												)
												(SimpleExpression : integer
													(Term : integer
														(Factor : integer
															(UnsignedConstant : integer
																(ConstantIdentifier : integer
																	(Identifier -> constant limit = 10
																		Letter 'l'
																		Letter 'i'
																		Letter 'm'
																		Letter 'i'
																		Letter 't'
																	)
																)
															)
														)
													)
												)
											)
										)
										Do 'do'
										(Statement
											(SimpleStatement
												(AssignmentStatement
													(VariableAccess : integer
														(EntireVariable : integer
															(VariableIdentifier : integer
																(Identifier -> variable i: integer
																	Letter 'i'
																)
															)
														)
													)
													Walrus ':='
													(Expression : integer
														(SimpleExpression : integer
															(Term : integer
																(Factor : integer
																	(FunctionDesignator : integer
																		(FunctionIdentifier
																			(Identifier -> function succ
																				Letter 's'
																				Letter 'u'
																				Letter 'c'
																				Letter 'c'
																			)
																		)
																		(ActualParameterList
																			OpenParen '('
																			(ActualParameter
																				(Expression : integer
																					(SimpleExpression : integer
																						(Term : integer
																							(Factor : integer
																								(VariableAccess : integer
																									(EntireVariable : integer
																										(VariableIdentifier : integer
																											(Identifier -> variable i: integer
																												Letter 'i'
																											)
																										)
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																			CloseParen ')'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(StructuredStatement
								(RepetitiveStatement
									(RepeatStatement
										Repeat 'repeat'
										(StatementSequence
											(Statement
												(SimpleStatement
													(AssignmentStatement
														(VariableAccess : integer
															(EntireVariable : integer
																(VariableIdentifier : integer
																	(Identifier -> variable i: integer
																		Letter 'i'
																	)
																)
															)
														)
														Walrus ':='
														(Expression : integer
															(SimpleExpression : integer
																(Term : integer
																	(Factor : integer
																		(FunctionDesignator : integer
																			(FunctionIdentifier
																				(Identifier -> function pred
																					Letter 'p'
																					Letter 'r'
																					Letter 'e'
																					Letter 'd'
																				)
																			)
																			(ActualParameterList
																				OpenParen '('
																				(ActualParameter
																					(Expression : integer
																						(SimpleExpression : integer
																							(Term : integer
																								(Factor : integer
																									(VariableAccess : integer
																										(EntireVariable : integer
																											(VariableIdentifier : integer
																												(Identifier -> variable i: integer
																													Letter 'i'
																												)
																											)
																										)
																									)
																								)
																							)
																						)
																					)
																				)
																				CloseParen ')'
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										Until 'until'
										(BooleanExpression : Boolean
											(Expression : Boolean
												(SimpleExpression : integer
													(Term : integer
														(Factor : integer
															(VariableAccess : integer
																(EntireVariable : integer
																	(VariableIdentifier : integer
																		(Identifier -> variable i: integer
																			Letter 'i'
																		)
																	)
																)
															)
														)
													)
												)
												(RelationalOperator
													LessThanOrEqual '<='
												)
												(SimpleExpression : integer
													(Term : integer
														(Factor : integer
															(UnsignedConstant : integer
																(UnsignedNumber : integer
																	(UnsignedInteger : integer
																		(DigitSequence
																			Digit '0'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(StructuredStatement
								(RepetitiveStatement
									(ForStatement
										For 'for'
										(ControlVariable : small
											(EntireVariable : small
												(VariableIdentifier : small
													(Identifier -> variable n: small
														Letter 'n'
													)
												)
											)
										)
										Walrus ':='
										(InitialValue : integer
											(Expression : integer
												(SimpleExpression : integer
													(Term : integer
														(Factor : integer
															(UnsignedConstant : integer
																(UnsignedNumber : integer
																	(UnsignedInteger : integer
																		(DigitSequence
																			Digit '1'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										To 'to'
										(FinalValue : integer
											(Expression : integer
												(SimpleExpression : integer
													(Term : integer
														(Factor : integer
															(UnsignedConstant : integer
																(ConstantIdentifier : integer
																	(Identifier -> constant limit = 10
																		Letter 'l'
																		Letter 'i'
																		Letter 'm'
																		Letter 'i'
																		Letter 't'
																	)
																)
															)
														)
													)
												)
											)
										)
										Do 'do'
										(Statement
											(SimpleStatement
												(AssignmentStatement
													(VariableAccess : integer
														(EntireVariable : integer
															(VariableIdentifier : integer
																(Identifier -> variable j: integer
																	Letter 'j'
																)
															)
														)
													)
													Walrus ':='
													(Expression : integer
														(SimpleExpression : integer
															(Term : integer
																(Factor : integer
																	(VariableAccess : integer
																		(EntireVariable : integer
																			(VariableIdentifier : integer
																				(Identifier -> variable j: integer
																					Letter 'j'
																				)
																			)
																		)
																	)
																)
															)
															(AddingOperator
																Plus '+'
															)
															(Term : small
																(Factor : small
																	(VariableAccess : small
																		(EntireVariable : small
																			(VariableIdentifier : small
																				(Identifier -> variable n: small
																					Letter 'n'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(StructuredStatement
								(RepetitiveStatement
									(ForStatement
										For 'for'
										(ControlVariable : colour
											(EntireVariable : colour
												(VariableIdentifier : colour
													(Identifier -> variable hue: colour
														Letter 'h'
														Letter 'u'
														Letter 'e'
													)
												)
											)
										)
										Walrus ':='
										(InitialValue : colour
											(Expression : colour
												(SimpleExpression : colour
													(Term : colour
														(Factor : colour
															(UnsignedConstant : colour
																(ConstantIdentifier : colour
																	(Identifier -> constant blue = blue
																		Letter 'b'
																		Letter 'l'
																		Letter 'u'
																		Letter 'e'
																	)
																)
															)
														)
													)
												)
											)
										)
										Downto 'downto'
										(FinalValue : colour
											(Expression : colour
												(SimpleExpression : colour
													(Term : colour
														(Factor : colour
															(UnsignedConstant : colour
																(ConstantIdentifier : colour
																	(Identifier -> constant red = red
																		Letter 'r'
																		Letter 'e'
																		Letter 'd'
																	)
																)
															)
														)
													)
												)
											)
										)
										Do 'do'
										(Statement
											(SimpleStatement
												(AssignmentStatement
													(VariableAccess : Boolean
														(EntireVariable : Boolean
															(VariableIdentifier : Boolean
																(Identifier -> variable b: Boolean
																	Letter 'b'
																)
															)
														)
													)
													Walrus ':='
													(Expression : Boolean
														(SimpleExpression : colour
															(Term : colour
																(Factor : colour
																	(VariableAccess : colour
																		(EntireVariable : colour
																			(VariableIdentifier : colour
																				(Identifier -> variable hue: colour
																					Letter 'h'
																					Letter 'u'
																					Letter 'e'
																				)
																			)
																		)
																	)
																)
															)
														)
														(RelationalOperator
															In 'in'
														)
														(SimpleExpression : set of colour
															(Term : set of colour
																(Factor : set of colour
																	(SetConstructor : set of colour
																		LeftSquareBracket '['
																		(MemberDesignator : colour
																			(Expression : colour
																				(SimpleExpression : colour
																					(Term : colour
																						(Factor : colour
																							(UnsignedConstant : colour
																								(ConstantIdentifier : colour
																									(Identifier -> constant red = red
																										Letter 'r'
																										Letter 'e'
																										Letter 'd'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																		)
																		Comma ','
																		(MemberDesignator : colour
																			(Expression : colour
																				(SimpleExpression : colour
																					(Term : colour
																						(Factor : colour
																							(UnsignedConstant : colour
																								(ConstantIdentifier : colour
																									(Identifier -> constant blue = blue
																										Letter 'b'
																										Letter 'l'
																										Letter 'u'
																										Letter 'e'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																		)
																		RightSquareBracket ']'
																	)
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(StructuredStatement
								(ConditionalStatement
									(CaseStatement
										Case 'case'
										(CaseIndex : colour
											(Expression : colour
												(SimpleExpression : colour
													(Term : colour
														(Factor : colour
															(VariableAccess : colour
																(EntireVariable : colour
																	(VariableIdentifier : colour
																		(Identifier -> variable hue: colour
																			Letter 'h'
																			Letter 'u'
																			Letter 'e'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										Of 'of'
										(CaseListElement
											(CaseConstantList
												(CaseConstant : colour
													(Constant : colour
														(ConstantIdentifier : colour
															(Identifier -> constant red = red
																Letter 'r'
																Letter 'e'
																Letter 'd'
															)
														)
													)
												)
											)
											Colon ':'
											(Statement
												(SimpleStatement
													(AssignmentStatement
														(VariableAccess : real
															(EntireVariable : real
																(VariableIdentifier : real
																	(Identifier -> variable x: real
																		Letter 'x'
																	)
																)
															)
														)
														Walrus ':='
														(Expression : real
															(SimpleExpression : real
																(Term : real
																	(Factor : real
																		(UnsignedConstant : real
																			(UnsignedNumber : real
																				(UnsignedReal : real
																					(DigitSequence
																						Digit '1'
																					)
																					Dot '.'
																					(FractionalPart
																						(DigitSequence
																							Digit '0'
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										Semicolon ';'
										(CaseListElement
											(CaseConstantList
												(CaseConstant : colour
													(Constant : colour
														(ConstantIdentifier : colour
															(Identifier -> constant green = green
																Letter 'g'
																Letter 'r'
																Letter 'e'
																Letter 'e'
																Letter 'n'
															)
														)
													)
												)
												Comma ','
												(CaseConstant : colour
													(Constant : colour
														(ConstantIdentifier : colour
															(Identifier -> constant blue = blue
																Letter 'b'
																Letter 'l'
																Letter 'u'
																Letter 'e'
															)
														)
													)
												)
											)
											Colon ':'
											(Statement
												(SimpleStatement
													(AssignmentStatement
														(VariableAccess : real
															(EntireVariable : real
																(VariableIdentifier : real
																	(Identifier -> variable x: real
																		Letter 'x'
																	)
																)
															)
														)
														Walrus ':='
														(Expression : real
															(SimpleExpression : real
																(Term : real
																	(Factor : real
																		(UnsignedConstant : real
																			(UnsignedNumber : real
																				(UnsignedReal : real
																					(DigitSequence
																						Digit '2'
																					)
																					Dot '.'
																					(FractionalPart
																						(DigitSequence
																							Digit '5'
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										End 'end'
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : real
										(EntireVariable : real
											(VariableIdentifier : real
												(Identifier -> variable x: real
													Letter 'x'
												)
											)
										)
									)
									Walrus ':='
									(Expression : real
										(SimpleExpression : real
											(Term : real
												(Factor : real
													(FunctionDesignator : real
														(FunctionIdentifier
															(Identifier -> function abs
																Letter 'a'
																Letter 'b'
																Letter 's'
															)
														)
														(ActualParameterList
															OpenParen '('
															(ActualParameter
																(Expression : real
																	(SimpleExpression : real
																		(Term : real
																			(Factor : real
																				(VariableAccess : real
																					(EntireVariable : real
																						(VariableIdentifier : real
																							(Identifier -> variable x: real
																								Letter 'x'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
															CloseParen ')'
														)
													)
												)
											)
											(AddingOperator
												Plus '+'
											)
											(Term : real
												(Factor : real
													(FunctionDesignator : real
														(FunctionIdentifier
															(Identifier -> function sqr
																Letter 's'
																Letter 'q'
																Letter 'r'
															)
														)
														(ActualParameterList
															OpenParen '('
															(ActualParameter
																(Expression : real
																	(SimpleExpression : real
																		(Term : real
																			(Factor : real
																				(VariableAccess : real
																					(EntireVariable : real
																						(VariableIdentifier : real
																							(Identifier -> variable y: real
																								Letter 'y'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
															CloseParen ')'
														)
													)
												)
											)
											(AddingOperator
												Plus '+'
											)
											(Term : real
												(Factor : real
													(FunctionDesignator : real
														(FunctionIdentifier
															(Identifier -> function sin: real
																Letter 's'
																Letter 'i'
																Letter 'n'
															)
														)
														(ActualParameterList
															OpenParen '('
															(ActualParameter
																(Expression : real
																	(SimpleExpression : real
																		(Term : real
																			(Factor : real
																				(VariableAccess : real
																					(EntireVariable : real
																						(VariableIdentifier : real
																							(Identifier -> variable x: real
																								Letter 'x'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
															CloseParen ')'
														)
													)
												)
											)
											(AddingOperator
												Plus '+'
											)
											(Term : real
												(Factor : real
													(FunctionDesignator : real
														(FunctionIdentifier
															(Identifier -> function cos: real
																Letter 'c'
																Letter 'o'
																Letter 's'
															)
														)
														(ActualParameterList
															OpenParen '('
															(ActualParameter
																(Expression : real
																	(SimpleExpression : real
																		(Term : real
																			(Factor : real
																				(VariableAccess : real
																					(EntireVariable : real
																						(VariableIdentifier : real
																							(Identifier -> variable x: real
																								Letter 'x'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
															CloseParen ')'
														)
													)
												)
											)
											(AddingOperator
												Plus '+'
											)
											(Term : real
												(Factor : real
													(FunctionDesignator : real
														(FunctionIdentifier
															(Identifier -> function exp: real
																Letter 'e'
																Letter 'x'
																Letter 'p'
															)
														)
														(ActualParameterList
															OpenParen '('
															(ActualParameter
																(Expression : real
																	(SimpleExpression : real
																		(Term : real
																			(Factor : real
																				(VariableAccess : real
																					(EntireVariable : real
																						(VariableIdentifier : real
																							(Identifier -> variable x: real
																								Letter 'x'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
															CloseParen ')'
														)
													)
												)
											)
											(AddingOperator
												Plus '+'
											)
											(Term : real
												(Factor : real
													(FunctionDesignator : real
														(FunctionIdentifier
															(Identifier -> function ln: real
																Letter 'l'
																Letter 'n'
															)
														)
														(ActualParameterList
															OpenParen '('
															(ActualParameter
																(Expression : real
																	(SimpleExpression : real
																		(Term : real
																			(Factor : real
																				(VariableAccess : real
																					(EntireVariable : real
																						(VariableIdentifier : real
																							(Identifier -> variable y: real
																								Letter 'y'
																							)
																						)
																					)
																				)
																			)
																		)
																		(AddingOperator
																			Plus '+'
																		)
																		(Term : integer
																			(Factor : integer
																				(UnsignedConstant : integer
																					(UnsignedNumber : integer
																						(UnsignedInteger : integer
																							(DigitSequence
																								Digit '1'
																								Digit '0'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
															CloseParen ')'
														)
													)
												)
											)
											(AddingOperator
												Plus '+'
											)
											(Term : real
												(Factor : real
													(FunctionDesignator : real
														(FunctionIdentifier
															(Identifier -> function sqrt: real
																Letter 's'
																Letter 'q'
																Letter 'r'
																Letter 't'
															)
														)
														(ActualParameterList
															OpenParen '('
															(ActualParameter
																(Expression : real
																	(SimpleExpression : real
																		(Term : real
																			(Factor : real
																				(FunctionDesignator : real
																					(FunctionIdentifier
																						(Identifier -> function abs
																							Letter 'a'
																							Letter 'b'
																							Letter 's'
																						)
																					)
																					(ActualParameterList
																						OpenParen '('
																						(ActualParameter
																							(Expression : real
																								(SimpleExpression : real
																									(Term : real
																										(Factor : real
																											(VariableAccess : real
																												(EntireVariable : real
																													(VariableIdentifier : real
																														(Identifier -> variable x: real
																															Letter 'x'
																														)
																													)
																												)
																											)
																										)
																									)
																								)
																							)
																						)
																						CloseParen ')'
																					)
																				)
																			)
																		)
																	)
																)
															)
															CloseParen ')'
														)
													)
												)
											)
											(AddingOperator
												Plus '+'
											)
											(Term : real
												(Factor : real
													(FunctionDesignator : real
														(FunctionIdentifier
															(Identifier -> function arctan: real
																Letter 'a'
																Letter 'r'
																Letter 'c'
																Letter 't'
																Letter 'a'
																Letter 'n'
															)
														)
														(ActualParameterList
															OpenParen '('
															(ActualParameter
																(Expression : real
																	(SimpleExpression : real
																		(Term : real
																			(Factor : real
																				(VariableAccess : real
																					(EntireVariable : real
																						(VariableIdentifier : real
																							(Identifier -> variable x: real
																								Letter 'x'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
															CloseParen ')'
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : integer
										(EntireVariable : integer
											(VariableIdentifier : integer
												(Identifier -> variable i: integer
													Letter 'i'
												)
											)
										)
									)
									Walrus ':='
									(Expression : integer
										(SimpleExpression : integer
											(Term : integer
												(Factor : integer
													(FunctionDesignator : integer
														(FunctionIdentifier
															(Identifier -> function trunc: integer
																Letter 't'
																Letter 'r'
																Letter 'u'
																Letter 'n'
																Letter 'c'
															)
														)
														(ActualParameterList
															OpenParen '('
															(ActualParameter
																(Expression : real
																	(SimpleExpression : real
																		(Term : real
																			(Factor : real
																				(VariableAccess : real
																					(EntireVariable : real
																						(VariableIdentifier : real
																							(Identifier -> variable x: real
																								Letter 'x'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
															CloseParen ')'
														)
													)
												)
											)
											(AddingOperator
												Plus '+'
											)
											(Term : integer
												(Factor : integer
													(FunctionDesignator : integer
														(FunctionIdentifier
															(Identifier -> function round: integer
																Letter 'r'
																Letter 'o'
																Letter 'u'
																Letter 'n'
																Letter 'd'
															)
														)
														(ActualParameterList
															OpenParen '('
															(ActualParameter
																(Expression : real
																	(SimpleExpression : real
																		(Term : real
																			(Factor : real
																				(VariableAccess : real
																					(EntireVariable : real
																						(VariableIdentifier : real
																							(Identifier -> variable y: real
																								Letter 'y'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
															CloseParen ')'
														)
													)
												)
											)
											(AddingOperator
												Plus '+'
											)
											(Term : integer
												(Factor : integer
													(FunctionDesignator : integer
														(FunctionIdentifier
															(Identifier -> function abs
																Letter 'a'
																Letter 'b'
																Letter 's'
															)
														)
														(ActualParameterList
															OpenParen '('
															(ActualParameter
																(Expression : integer
																	(SimpleExpression : integer
																		(Term : integer
																			(Factor : integer
																				(VariableAccess : integer
																					(EntireVariable : integer
																						(VariableIdentifier : integer
																							(Identifier -> variable i: integer
																								Letter 'i'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
															CloseParen ')'
														)
													)
												)
											)
											(AddingOperator
												Plus '+'
											)
											(Term : integer
												(Factor : integer
													(FunctionDesignator : integer
														(FunctionIdentifier
															(Identifier -> function sqr
																Letter 's'
																Letter 'q'
																Letter 'r'
															)
														)
														(ActualParameterList
															OpenParen '('
															(ActualParameter
																(Expression : integer
																	(SimpleExpression : integer
																		(Term : integer
																			(Factor : integer
																				(VariableAccess : integer
																					(EntireVariable : integer
																						(VariableIdentifier : integer
																							(Identifier -> variable i: integer
																								Letter 'i'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
															CloseParen ')'
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : char
										(EntireVariable : char
											(VariableIdentifier : char
												(Identifier -> variable c: char
													Letter 'c'
												)
											)
										)
									)
									Walrus ':='
									(Expression : char
										(SimpleExpression : char
											(Term : char
												(Factor : char
													(FunctionDesignator : char
														(FunctionIdentifier
															(Identifier -> function chr: char
																Letter 'c'
																Letter 'h'
																Letter 'r'
															)
														)
														(ActualParameterList
															OpenParen '('
															(ActualParameter
																(Expression : integer
																	(SimpleExpression : integer
																		(Term : integer
																			(Factor : integer
																				(FunctionDesignator : integer
																					(FunctionIdentifier
																						(Identifier -> function ord: integer
																							Letter 'o'
																							Letter 'r'
																							Letter 'd'
																						)
																					)
																					(ActualParameterList
																						OpenParen '('
																						(ActualParameter
																							(Expression : char
																								(SimpleExpression : char
																									(Term : char
																										(Factor : char
																											(VariableAccess : char
																												(EntireVariable : char
																													(VariableIdentifier : char
																														(Identifier -> variable c: char
																															Letter 'c'
																														)
																													)
																												)
																											)
																										)
																									)
																								)
																							)
																						)
																						CloseParen ')'
																					)
																				)
																			)
																		)
																		(AddingOperator
																			Plus '+'
																		)
																		(Term : integer
																			(Factor : integer
																				(UnsignedConstant : integer
																					(UnsignedNumber : integer
																						(UnsignedInteger : integer
																							(DigitSequence
																								Digit '1'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
															CloseParen ')'
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(AssignmentStatement
									(VariableAccess : Boolean
										(EntireVariable : Boolean
											(VariableIdentifier : Boolean
												(Identifier -> variable b: Boolean
													Letter 'b'
												)
											)
										)
									)
									Walrus ':='
									(Expression : Boolean
										(SimpleExpression : Boolean
											(Term : Boolean
												(Factor : Boolean
													(FunctionDesignator : Boolean
														(FunctionIdentifier
															(Identifier -> function odd: Boolean
																Letter 'o'
																Letter 'd'
																Letter 'd'
															)
														)
														(ActualParameterList
															OpenParen '('
															(ActualParameter
																(Expression : integer
																	(SimpleExpression : integer
																		(Term : integer
																			(Factor : integer
																				(VariableAccess : integer
																					(EntireVariable : integer
																						(VariableIdentifier : integer
																							(Identifier -> variable i: integer
																								Letter 'i'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
															CloseParen ')'
														)
													)
												)
												(MultiplyingOperator
													And 'and'
												)
												(Factor : Boolean
													OpenParen '('
													(Expression : Boolean
														(SimpleExpression : char
															(Term : char
																(Factor : char
																	(VariableAccess : char
																		(EntireVariable : char
																			(VariableIdentifier : char
																				(Identifier -> variable c: char
																					Letter 'c'
																				)
																			)
																		)
																	)
																)
															)
														)
														(RelationalOperator
															KetPair '<>'
														)
														(SimpleExpression : char
															(Term : char
																(Factor : char
																	(UnsignedConstant : char
																		CharacterString ''z''
																	)
																)
															)
														)
													)
													CloseParen ')'
												)
												(MultiplyingOperator
													And 'and'
												)
												(Factor : Boolean
													OpenParen '('
													(Expression : Boolean
														(SimpleExpression : packed array[1..5] of char
															(Term : packed array[1..5] of char
																(Factor : packed array[1..5] of char
																	(UnsignedConstant : packed array[1..5] of char
																		(ConstantIdentifier : packed array[1..5] of char
																			(Identifier -> constant greeting = 'hello'
																				Letter 'g'
																				Letter 'r'
																				Letter 'e'
																				Letter 'e'
																				Letter 't'
																				Letter 'i'
																				Letter 'n'
																				Letter 'g'
																			)
																		)
																	)
																)
															)
														)
														(RelationalOperator
															LessThan '<'
														)
														(SimpleExpression : packed array[1..5] of char
															(Term : packed array[1..5] of char
																(Factor : packed array[1..5] of char
																	(UnsignedConstant : packed array[1..5] of char
																		CharacterString ''world''
																	)
																)
															)
														)
													)
													CloseParen ')'
												)
												(MultiplyingOperator
													And 'and'
												)
												(Factor : Boolean
													OpenParen '('
													(Expression : Boolean
														(SimpleExpression : packed array[1..5] of char
															(Term : packed array[1..5] of char
																(Factor : packed array[1..5] of char
																	(UnsignedConstant : packed array[1..5] of char
																		(ConstantIdentifier : packed array[1..5] of char
																			(Identifier -> constant greeting = 'hello'
																				Letter 'g'
																				Letter 'r'
																				Letter 'e'
																				Letter 'e'
																				Letter 't'
																				Letter 'i'
																				Letter 'n'
																				Letter 'g'
																			)
																		)
																	)
																)
															)
														)
														(RelationalOperator
															Equals '='
														)
														(SimpleExpression : packed array[1..5] of char
															(Term : packed array[1..5] of char
																(Factor : packed array[1..5] of char
																	(UnsignedConstant : packed array[1..5] of char
																		CharacterString ''hello''
																	)
																)
															)
														)
													)
													CloseParen ')'
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(ProcedureStatement
									(ProcedureIdentifier
										(Identifier -> procedure read
											Letter 'r'
											Letter 'e'
											Letter 'a'
											Letter 'd'
										)
									)
									(ReadParameterList
										OpenParen '('
										(VariableAccess : integer
											(EntireVariable : integer
												(VariableIdentifier : integer
													(Identifier -> variable i: integer
														Letter 'i'
													)
												)
											)
										)
										Comma ','
										(VariableAccess : real
											(EntireVariable : real
												(VariableIdentifier : real
													(Identifier -> variable x: real
														Letter 'x'
													)
												)
											)
										)
										Comma ','
										(VariableAccess : char
											(EntireVariable : char
												(VariableIdentifier : char
													(Identifier -> variable c: char
														Letter 'c'
													)
												)
											)
										)
										CloseParen ')'
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(ProcedureStatement
									(ProcedureIdentifier
										(Identifier -> procedure readln
											Letter 'r'
											Letter 'e'
											Letter 'a'
											Letter 'd'
											Letter 'l'
											Letter 'n'
										)
									)
									(ReadlnParameterList)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(ProcedureStatement
									(ProcedureIdentifier
										(Identifier -> procedure write
											Letter 'w'
											Letter 'r'
											Letter 'i'
											Letter 't'
											Letter 'e'
										)
									)
									(WriteParameterList
										OpenParen '('
										(WriteParameter
											(Expression : integer
												(SimpleExpression : integer
													(Term : integer
														(Factor : integer
															(VariableAccess : integer
																(EntireVariable : integer
																	(VariableIdentifier : integer
																		(Identifier -> variable i: integer
																			Letter 'i'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										Comma ','
										(WriteParameter
											(Expression : char
												(SimpleExpression : char
													(Term : char
														(Factor : char
															(UnsignedConstant : char
																CharacterString '' ''
															)
														)
													)
												)
											)
										)
										Comma ','
										(WriteParameter
											(Expression : real
												(SimpleExpression : real
													(Term : real
														(Factor : real
															(VariableAccess : real
																(EntireVariable : real
																	(VariableIdentifier : real
																		(Identifier -> variable x: real
																			Letter 'x'
																		)
																	)
																)
															)
														)
													)
												)
											)
											Colon ':'
											(Expression : integer
												(SimpleExpression : integer
													(Term : integer
														(Factor : integer
															(UnsignedConstant : integer
																(UnsignedNumber : integer
																	(UnsignedInteger : integer
																		(DigitSequence
																			Digit '1'
																			Digit '0'
																		)
																	)
																)
															)
														)
													)
												)
											)
											Colon ':'
											(Expression : integer
												(SimpleExpression : integer
													(Term : integer
														(Factor : integer
															(UnsignedConstant : integer
																(UnsignedNumber : integer
																	(UnsignedInteger : integer
																		(DigitSequence
																			Digit '2'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										Comma ','
										(WriteParameter
											(Expression : char
												(SimpleExpression : char
													(Term : char
														(Factor : char
															(UnsignedConstant : char
																CharacterString '' ''
															)
														)
													)
												)
											)
										)
										Comma ','
										(WriteParameter
											(Expression : char
												(SimpleExpression : char
													(Term : char
														(Factor : char
															(VariableAccess : char
																(EntireVariable : char
																	(VariableIdentifier : char
																		(Identifier -> variable c: char
																			Letter 'c'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										Comma ','
										(WriteParameter
											(Expression : Boolean
												(SimpleExpression : Boolean
													(Term : Boolean
														(Factor : Boolean
															(VariableAccess : Boolean
																(EntireVariable : Boolean
																	(VariableIdentifier : Boolean
																		(Identifier -> variable b: Boolean
																			Letter 'b'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										Comma ','
										(WriteParameter
											(Expression : packed array[1..5] of char
												(SimpleExpression : packed array[1..5] of char
													(Term : packed array[1..5] of char
														(Factor : packed array[1..5] of char
															(UnsignedConstant : packed array[1..5] of char
																(ConstantIdentifier : packed array[1..5] of char
																	(Identifier -> constant greeting = 'hello'
																		Letter 'g'
																		Letter 'r'
																		Letter 'e'
																		Letter 'e'
																		Letter 't'
																		Letter 'i'
																		Letter 'n'
																		Letter 'g'
																	)
																)
															)
														)
													)
												)
											)
											Colon ':'
											(Expression : integer
												(SimpleExpression : integer
													(Term : integer
														(Factor : integer
															(UnsignedConstant : integer
																(UnsignedNumber : integer
																	(UnsignedInteger : integer
																		(DigitSequence
																			Digit '8'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										CloseParen ')'
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(ProcedureStatement
									(ProcedureIdentifier
										(Identifier -> procedure writeln
											Letter 'w'
											Letter 'r'
											Letter 'i'
											Letter 't'
											Letter 'e'
											Letter 'l'
											Letter 'n'
										)
									)
									(WritelnParameterList
										OpenParen '('
										(WriteParameter
											(Expression : integer
												(SimpleExpression : integer
													(Term : integer
														(Factor : integer
															(VariableAccess : integer
																(EntireVariable : integer
																	(VariableIdentifier : integer
																		(Identifier -> variable i: integer
																			Letter 'i'
																		)
																	)
																)
															)
														)
													)
												)
											)
											Colon ':'
											(Expression : integer
												(SimpleExpression : integer
													(Term : integer
														(Factor : integer
															(UnsignedConstant : integer
																(UnsignedNumber : integer
																	(UnsignedInteger : integer
																		(DigitSequence
																			Digit '5'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										CloseParen ')'
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(ProcedureStatement
									(ProcedureIdentifier
										(Identifier -> procedure writeln
											Letter 'w'
											Letter 'r'
											Letter 'i'
											Letter 't'
											Letter 'e'
											Letter 'l'
											Letter 'n'
										)
									)
									(WritelnParameterList)
								)
							)
						)
					)
					End 'end'
				)
			)
		)
	)
	Dot '.'
)